using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using ArtDatabanken.Data;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonInfoQualityChartModelHelper
    {
        public MemoryStream GetQualityChart(IList<ITaxonChildQualityStatistics> taxonQualitySummaryList)
        {
            QualityChartLabels labels = new QualityChartLabels();
             
            var chart = new Chart();
            var area = new ChartArea();
            var legend = new Legend();
            
            // create and customize your data series.
            var series = new Series();
            
            // configure chart area (dimensions, etc) here.
            chart.ChartAreas.Add(area);
            
            // 3D
            //chart.ChartAreas[area.Name].Area3DStyle.Enable3D = true;
            
            chart.Width = Unit.Pixel(245);
            chart.Height = Unit.Pixel(225);
            
            // change theme/palette
            //series.Palette = ChartColorPalette.Fire;

            Int32 i = 0;
            SluColors colorScheme = new SluColors();

            foreach (var item in taxonQualitySummaryList)
            {
                string chartLabel = labels.QualityChartLabelArray[i];

                if (item.ChildTaxaCount > 0)
                {   
                    series.Points.Add(new DataPoint
                    {
                        //Label = chartLabel + " #PERCENT{P}",  //Name + Percent
                        LegendText = chartLabel + ": " + item.ChildTaxaCount, 
                        Color = colorScheme.SluColorArray[i],
                        YValues = new double[] { item.ChildTaxaCount }
                    });
                }
                else
                {
                    series.Points.Add(new DataPoint
                    {
                        LegendText = chartLabel + ": " + item.ChildTaxaCount,
                        Color = colorScheme.SluColorArray[i],
                        YValues = new double[] { item.ChildTaxaCount }
                    });
                }
                i++;
            }
            
            // Show percent on all labels
            // series.Label = "#PERCENT{P0}";

            // Cut out
            //series.Points[0]["Exploded"] = "true";

            series.Font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            
            series.ChartType = SeriesChartType.Pie;
            //series["PieLabelStyle"] = "Inside";
            series["PieLabelStyle"] = "Outside";

            series.BorderWidth = 1;
            series.BorderColor = colorScheme.SluColorArray[9];

            legend.Docking = Docking.Bottom;
            legend.Enabled = true;
            legend.Alignment = StringAlignment.Center;
            legend.BorderWidth = 0;
            legend.TableStyle = LegendTableStyle.Auto;
            
            chart.Legends.Add(legend);
            
            chart.Series.Add(series);

            var returnStream = new MemoryStream();
            chart.ImageType = ChartImageType.Png;
            chart.SaveImage(returnStream);

            returnStream.Position = 0;
            
            return returnStream;
        }
    }

    /// <summary>
    /// Localized labels used in GetQualityChart
    /// </summary>
    public class QualityChartLabels
    {
        public String[] QualityChartLabelArray = new String[4]
                                                      {
                                                          Resources.DyntaxaResource.TaxonInfoQuality0Label,
                                                          Resources.DyntaxaResource.TaxonInfoQuality1Label,
                                                          Resources.DyntaxaResource.TaxonInfoQuality2Label,
                                                          Resources.DyntaxaResource.TaxonInfoQuality3Label
                                                      };
    }

    public class SluColors
    {
        // TODO: Make help class / hash table or such 
        public Color[] SluColorArray = new Color[10]
                                         {
                                             Color.FromArgb(204, 199, 192), //SLU Warm Gray
                                             Color.FromArgb(210, 142, 0), //SLU Orange
                                             Color.FromArgb(218, 214, 102), //SLU Lime 60%
                                             Color.FromArgb(128, 191, 211), //SLU Blue 60%
                                             Color.FromArgb(228, 187, 102), // SLU Orange 60%
                                             Color.FromArgb(194, 160, 219), // SLU Purple 60%
                                             Color.FromArgb(193, 187, 0), //SLU Lime
                                             Color.FromArgb(44, 149, 181), //SLU Blue
                                             Color.FromArgb(153, 97, 195), //SLU Purple
                                             Color.FromArgb(97, 98, 101), //SLU Cool Gray
                                         };
    }
}
