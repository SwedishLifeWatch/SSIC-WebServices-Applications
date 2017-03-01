using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.GIS.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Result
{
    public static class MapExportViewManager
    {
        public static byte[] GetMapImage(HttpServerUtilityBase server, MapExportModel model, string qgisPath, string pythonPath)
        {
            var tempPath = server.MapPath(FileSystemManager.GetTempDirectoryPath());
            var baseLayerPath = server.MapPath("~/Content/MapExport/baseLayer.geojson");
            var concernedFiles = new List<string>();
            byte[] exportFileData = null;
            Process process = null;
            string stdoutx = null;
            string stderrx = null;

            try
            {
                FileSystemManager.EnsureFolderExists(tempPath);

                var sb = new StringBuilder();

                //Init python script
                InitScript(sb, qgisPath, model.MapExtent);

                //Add base layer
                AddLayer(sb, baseLayerPath, "Baselayer", 255, false, false, new MapExportModel.LegendItem[] { new MapExportModel.LegendItem { Color = "#FAFAFA", Name = "Sweden" } });

                //Make sure layers are sorted by z-index
                model.Layers = model.Layers.OrderBy(l => l.Zindex).ToArray();
               
                //Add all other layers
                foreach (var layer in model.Layers)
                {
                    //Create file path
                    var layerFilePath = FileSystemManager.CombinePathAndFilename(tempPath,
                        FileSystemManager.CreateRandomFilename(".geojson"));
                    concernedFiles.Add(layerFilePath);

                    //Save layer geojson in temp file
                    FileSystemManager.CreateTextFile(layerFilePath, layer.GeoJson);

                    //Add layer to script
                    AddLayer(sb, layerFilePath, layer.Name.ReplaceSwedishChars().RemoveNonAscii(), layer.Occupancy, layer.IsPointLayer, true, layer.Legends, layer.Attribute);
                }
                
                //Create temp output path
                var outputFilePath = FileSystemManager.CombinePathAndFilename(tempPath,
                    FileSystemManager.CreateRandomFilename(".png"));
                concernedFiles.Add(outputFilePath);

                //Finalize script
                FinalizeScript(sb, model.Dpi, outputFilePath);

                //Create python script path
                var pythonScriptPath = FileSystemManager.CombinePathAndFilename(tempPath,
                    FileSystemManager.CreateRandomFilename(".py"));
                concernedFiles.Add(pythonScriptPath);

                //Temporaly save script file
                FileSystemManager.CreateTextFile(pythonScriptPath, sb.ToString());

                var command = string.Format("{0}\\python", pythonPath);
                var arguments = pythonScriptPath;

                //Create new process
                process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = arguments,
                        CreateNoWindow = true,
                        FileName = command,
                        UseShellExecute = false,
                        WorkingDirectory = pythonPath
                    }
                };
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                //Start process
                process.Start();

                //Wait max 1 min to execute
                var timeout = TimeSpan.FromMinutes(1);
                stdoutx = process.StandardOutput.ReadToEnd();
                stderrx = process.StandardError.ReadToEnd();                
                if (process.WaitForExit((int) timeout.TotalMilliseconds)) // wait max 1 minutes.
                {
                    exportFileData = System.IO.File.ReadAllBytes(outputFilePath);
                }
                else // timeout
                {
                    process.Kill();
                    throw new Exception("Python process Timeout");
                }
            }
            catch
            {                
                if (process != null)
                {                    
                    Debug.WriteLine("QGIS Map generation Exit code : {0}", process.ExitCode);
                    Debug.WriteLine("QGIS Map generation Stdout : {0}", stdoutx);
                    Debug.WriteLine("QGIS Map generation Stderr : {0}", stderrx);
                }
                
                //Clean up
                if (process != null && !process.HasExited)
                {                    
                    process.Kill();
                }
            }
            finally
            {
                //Remove all files created in this method
                foreach (var concernedFile in concernedFiles)
                {
                    FileSystemManager.DeleteFile(concernedFile);
                }
            }

            return exportFileData;
        }

        private static void InitScript(StringBuilder sb, string qgisPath, MapExportModel.Extent extent)
        {            
            sb.AppendLine("from qgis.core import QgsApplication, QgsMapRenderer, QgsMapLayerRegistry, QgsLayerTreeGroup, QgsVectorLayer, QgsRectangle, QgsComposition, QgsComposerMap, QgsComposerLegend, QgsComposerScaleBar, QgsComposerLabel, QgsComposerLegendStyle, QgsMarkerSymbolV2, QgsSymbolV2, QgsRendererRangeV2, QgsGraduatedSymbolRendererV2");
            sb.AppendLine("from PyQt4.QtCore import QSize, QRectF");
            sb.AppendLine("from PyQt4.QtGui import QImage, QPainter, QColor, QFont");
            sb.AppendLine("");

            //Function for getting time stamp of layer id
            sb.AppendLine("def getTimeStamp(uStr):");
            sb.AppendLine("\treturn str(uStr)[len(uStr) - 11:]");
            sb.AppendLine("");

            sb.AppendLine("qgs = QgsApplication([], False)");
            sb.AppendFormat("qgs.setPrefixPath(\"{0}\", True)", qgisPath.Replace(@"\", @"\\"));
            sb.AppendLine("");
            sb.AppendLine("qgs.initQgis()");
            sb.AppendLine("");

            sb.AppendLine("instance = QgsMapLayerRegistry.instance()");
            sb.AppendFormat("mapRectangle = QgsRectangle({0}, {1}, {2}, {3})", extent.Left.ToString().Replace(",", "."), extent.Bottom.ToString().Replace(",", "."), extent.Right.ToString().Replace(",", "."), extent.Top.ToString().Replace(",", "."));
            sb.AppendLine("");
            //sb.AppendLine("widthInKm = mapRectangle.width() / 1000");
            //sb.AppendLine("heightInKm = mapRectangle.height() / 1000");
            //Height and width in meter (SWEREF99) * 1000 equals mm / scale
           
            sb.AppendLine("legendLayers = QgsLayerTreeGroup()");
        }
       
        private static void AddLayer(StringBuilder sb, string path, string name, byte occupancy, bool isPointLayer, bool showInLegend, MapExportModel.LegendItem[] legends = null, string attribute = null)
        {
            sb.AppendFormat("vectorLayer = QgsVectorLayer(\"{0}\", \"{1}\", \"ogr\")", path.Replace(@"\", @"\\"), name);
            sb.AppendLine("");

            if (string.IsNullOrEmpty(attribute))
            {
                if (legends != null && legends.Length != 0 && legends[0].Color != null)
                {
                    var strColor = legends[0].Color.Replace("#", "");
                    var color = Color.FromArgb(
                        occupancy,
                        int.Parse(strColor.Substring(0, 2), NumberStyles.HexNumber),
                        int.Parse(strColor.Substring(2, 2), NumberStyles.HexNumber),
                        int.Parse(strColor.Substring(4, 2), NumberStyles.HexNumber));

                    if (isPointLayer)
                    {
                        sb.AppendLine(string.Format("symbol = QgsMarkerSymbolV2.createSimple({{'name': 'circle', 'color': '{0},{1},{2},{3}', 'size': '25'}})", color.R, color.G, color.B, color.A));
                        sb.AppendLine("vectorLayer.rendererV2().setSymbol(symbol)");
                    }
                    else
                    {
                        sb.AppendLine("symbol_layer = vectorLayer.rendererV2().symbols()[0].symbolLayer(0)");

                        if (legends[0].Color.StartsWith("#"))
                        {
                            sb.AppendFormat("symbol_layer.setColor(QColor({0}, {1}, {2}, {3}))", color.R, color.G, color.B, color.A);
                        }
                        else
                        {
                            sb.AppendFormat("symbol_layer.setColor(QColor(\"{0}\"))", legends[0].Color);
                        }
                    }
                    
                }
                
                sb.AppendLine("");
            }
            else
            {
                sb.AppendLine("legendValues = (");
                foreach (var legend in legends)
                {
                    if (legend.Color.StartsWith("#"))
                    {
                        var strColor = legend.Color.Replace("#", "");
                        var color = Color.FromArgb(
                            occupancy,
                            int.Parse(strColor.Substring(0, 2), NumberStyles.HexNumber),
                            int.Parse(strColor.Substring(2, 2), NumberStyles.HexNumber),
                            int.Parse(strColor.Substring(4, 2), NumberStyles.HexNumber));
                        
                        sb.AppendFormat("('{0}', {1}, {2}, QColor({3}, {4}, {5}, {6}))", legend.Name, legend.MinValue, legend.MaxValue, color.R, color.G, color.B, color.A);
                    }
                    else
                    {
                        sb.AppendFormat("('{0}', {1}, {2}, QColor('{3}'))", legend.Name, legend.MinValue, legend.MaxValue, legend.Color);
                    }

                    sb.AppendLine(",");
                }
                sb.AppendLine(")");

                sb.AppendLine("ranges = []");

                //create a category for each item in values
                sb.AppendLine("for label, lower, upper, color in legendValues:");
                sb.AppendLine("\tsymbol = QgsSymbolV2.defaultSymbol(vectorLayer.geometryType())");
                sb.AppendLine("\tsymbol.setColor(color)");
                sb.AppendLine("\trng = QgsRendererRangeV2(lower, upper, symbol, label)");
                sb.AppendLine("\tranges.append(rng)");
                
                //create the renderer and assign it to a layer
                sb.AppendFormat("attribute = '{0}'", attribute);
                sb.AppendLine("");
                sb.AppendLine("renderer = QgsGraduatedSymbolRendererV2(attribute, ranges)");
                sb.AppendLine("vectorLayer.setRendererV2(renderer)");
            }

            sb.AppendLine("instance.addMapLayer(vectorLayer, False)");

            if (showInLegend)
            {
                sb.AppendLine("legendLayers.addLayer(vectorLayer)");
            }

            sb.AppendLine("");
        }

        private static void AddLegend(StringBuilder sb)
        {
            sb.AppendLine("#--------- Start Legend ---------");
            sb.AppendLine("legend = QgsComposerLegend(composition)");
            sb.AppendLine("legend.modelV2().setRootGroup(legendLayers)");
            sb.AppendLine("legend.setTitle('')");
            sb.AppendLine("legend.setItemPosition(10, 10, 0, 0)");
            sb.AppendLine("legend.setSymbolHeight(fontSize)");
            sb.AppendLine("legend.setSymbolWidth(fontSize)");
            sb.AppendLine("legend.setStyleFont(QgsComposerLegendStyle.Title, QFont(\"Comic Sans MS\", 0))");
            sb.AppendLine("legend.setStyleFont(QgsComposerLegendStyle.Subgroup, QFont(\"Comic Sans MS\", fontSize))");
            sb.AppendLine("legend.setStyleFont(QgsComposerLegendStyle.SymbolLabel, QFont(\"Comic Sans MS\", fontSize))");
            sb.AppendLine("legend.setStyleMargin(QgsComposerLegendStyle.SymbolLabel, fontSize/3)");
            sb.AppendLine("legend.adjustBoxSize()");
            sb.AppendLine("composition.addItem(legend)");
            sb.AppendLine("#--------- End Legend ---------");
            sb.AppendLine("");
        }

        private static void AddScaleBar(StringBuilder sb)
        {
            sb.AppendLine("#--------- Start Scale Bar ---------");
            sb.AppendLine("scaleBar = QgsComposerScaleBar(composition)");
            sb.AppendLine("scaleBar.setComposerMap(composerMap)");
            sb.AppendLine("scaleBar.applyDefaultSize(1)");
            sb.AppendLine("scaleBar.setStyle('Single Box')");
            sb.AppendLine("scaleBar.setItemPosition(20, imageHeight - 40)");
            sb.AppendLine("scaleBar.setFont(QFont(\"Comic Sans MS\", fontSize))");
            sb.AppendLine("scaleBar.setHeight(fontSize/3)");
            sb.AppendLine("scaleBar.setUnitLabeling('km')");
            sb.AppendLine("numMapUnitsM = 111500000");
            sb.AppendLine("scaleBar.setNumMapUnitsPerScaleBarUnit(numMapUnitsM)");
            sb.AppendLine("widthInKm = mapRectangle.width() / 1000");
            sb.AppendLine("if widthInKm > 500:"); //More than 500 km
            sb.AppendLine("\tscaleBar.setNumUnitsPerSegment(numMapUnitsM * 100)");
            sb.AppendLine("elif widthInKm > 100:");
            sb.AppendLine("\tscaleBar.setNumUnitsPerSegment(numMapUnitsM * 10)");
            sb.AppendLine("else:");
            sb.AppendLine("\tscaleBar.setNumUnitsPerSegment(numMapUnitsM)");

            sb.AppendLine("composition.addItem(scaleBar)");
            sb.AppendLine("#--------- End Scale Bar ---------");
            sb.AppendLine("");
        }
    
        private static void AddCoordinateSystem(StringBuilder sb)
        {
            sb.AppendLine("#--------- Start Coordinate system ---------");
            sb.AppendLine("composerLabel = QgsComposerLabel(composition)");
            sb.AppendLine("composerLabel.setFont(QFont(\"Comic Sans MS\", fontSize))");
            sb.AppendLine("composerLabel.setItemPosition(imageWidth - 100, imageHeight - 20)");
            sb.AppendLine("composerLabel.setText(\"SWEREF 99 TM\")");
            sb.AppendLine("composerLabel.adjustSizeToText()");
            sb.AppendLine("composition.addItem(composerLabel)");
            sb.AppendLine("#--------- End Coordinate system ---------");
            sb.AppendLine("");
        }

        private static void FinalizeScript(StringBuilder sb, int dpi, string fileName)
        {
            sb.AppendLine("layerKeys = instance.mapLayers().keys()");
            sb.AppendLine("layerKeys.sort(key=getTimeStamp, reverse=True)");
            sb.AppendLine("");

            sb.AppendLine("mapRenderer = QgsMapRenderer()");
            sb.AppendLine("mapRenderer.setLayerSet(layerKeys)");
            sb.AppendLine("mapRenderer.setExtent(mapRectangle) ");
            sb.AppendLine("");

            sb.AppendLine("ratio = mapRectangle.width()/mapRectangle.height()");
            sb.AppendLine("composition = QgsComposition(mapRenderer)");
            sb.AppendLine("composition.setPlotStyle(QgsComposition.Print)");
            sb.AppendLine("composition.setPaperSize(ratio * 300, 300)");
            sb.AppendLine("");

            sb.AppendFormat("dpi = {0}", dpi);
            sb.AppendLine("");
            
            sb.AppendLine("composition.setPrintResolution(dpi)");
            sb.AppendLine("dpmm = dpi / 25.4");
            sb.AppendLine("imageWidth = int(dpmm * composition.paperWidth())");
            sb.AppendLine("imageHeight = int(dpmm * composition.paperHeight())");
            sb.AppendLine("x, y = 0, 0");
            sb.AppendLine("");

            sb.AppendLine("composerMap = QgsComposerMap(composition, x, y, imageWidth, imageHeight)");
            sb.AppendLine("composerMap.setNewExtent(mapRectangle)");
            sb.AppendLine("composerMap.setItemPosition(0, 0)");
            sb.AppendLine("");

            sb.AppendLine("composition.addItem(composerMap)");
            sb.AppendLine("");

            sb.AppendLine("fontSize = 36");
            sb.AppendLine("");

            AddLegend(sb);
            AddScaleBar(sb);
            AddCoordinateSystem(sb);

            sb.AppendLine("image = QImage(QSize(imageWidth, imageHeight), QImage.Format_ARGB32)");
            sb.AppendLine("image.setDotsPerMeterX(dpmm * 1000)");
            sb.AppendLine("image.setDotsPerMeterY(dpmm * 1000)");
            sb.AppendLine("image.fill(0)");
            sb.AppendLine("");

            sb.AppendLine("painter = QPainter(image)");
            sb.AppendLine("sourceArea = QRectF(0, 0, imageWidth, imageHeight)");
            sb.AppendLine("targetArea = QRectF(0, 0, imageWidth, imageHeight)");
            sb.AppendLine("composition.render(painter, targetArea, sourceArea) ");
            sb.AppendLine("painter.end()");
            sb.AppendFormat("image.save(\"{0}\", \"png\")", fileName.Replace(@"\", @"\\"));
            sb.AppendLine("");
            sb.AppendLine("qgs.exitQgis()");
        }
    }   
}