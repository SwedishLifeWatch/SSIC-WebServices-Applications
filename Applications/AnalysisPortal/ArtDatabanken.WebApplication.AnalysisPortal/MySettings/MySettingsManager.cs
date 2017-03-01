using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Hosting;
using ArtDatabanken.GIS.FormatConversion;
using System.Xml;
using ArtDatabanken.Data;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.CoordinateConversion;
using ArtDatabanken.GIS.GeoJSON.Net.Feature;
using ArtDatabanken.WebApplication.AnalysisPortal.Exceptions;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings
{
    /// <summary>
    /// This class handles loading and saving of MySettings object.
    /// It supports saving to disc, and in the future to db.
    /// </summary>
    public static class MySettingsManager
    {
        private const string FileExtension = ".dat";
        public const string SettingsName = "mysettings";
        private const string LastSettingsFileName = "last-settings";
        private const string MapDataDirectory = "mapdata";

        /// <summary>
        /// Replaces all illegal file name characters with '_'
        /// and returns a legal file name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string RemoveIllegalFileNameCharacters(string filename)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }
            return filename;
        }

        /// <summary>
        /// Gets the full file name including the path.
        /// </summary>
        /// <param name="settingsName">The name</param>
        /// <returns></returns>
        private static string GetFileName(string settingsName)
        {
            settingsName = settingsName.Trim().RemoveDuplicateBlanks();
            String settingsDirectory = Resources.AppSettings.Default.PathToSettingsDirectory;
            string fileName = RemoveIllegalFileNameCharacters(settingsName);
            fileName = Path.ChangeExtension(fileName, FileExtension);
            string filePath = HostingEnvironment.MapPath(Path.Combine(settingsDirectory, fileName));
            return filePath;
        }

        /// <summary>
        /// Gets the full file name including the path.
        /// </summary>
        /// <param name="settingsName">The name</param>
        /// <returns></returns>
        private static string GetFilePath(string settingsName, string userName)
        {
            settingsName = settingsName.Trim().RemoveDuplicateBlanks();
            var fileName = RemoveIllegalFileNameCharacters(settingsName);
            fileName = Path.ChangeExtension(fileName, FileExtension);
          
            return string.Format("{0}\\{1}", GetDirectoryPath(userName), fileName);
        }

        private static string GetDirectoryPath(string userName)
        {
            userName = RemoveIllegalFileNameCharacters(userName);
            var settingsDirectory = Resources.AppSettings.Default.PathToSettingsDirectory;
            return HostingEnvironment.MapPath(Path.Combine(settingsDirectory, userName));
        }

        /// <summary>
        /// Gets the MySettings folder server path.
        /// </summary>
        /// <returns></returns>
        private static string GetSettingsServerPath()
        {
            string settingsDirectory = Resources.AppSettings.Default.PathToSettingsDirectory;
            string path = HostingEnvironment.MapPath(settingsDirectory);
            return path;
        }

        /// <summary>
        /// Ensures that the folder on disk exists.
        /// If it doesn't exist, the folder will be created
        /// </summary>
        /// <param name="path">The path.</param>
        private static void EnsureFolder(string path) 
        { 
            string directoryName = Path.GetDirectoryName(path); 
            if (directoryName != null && ((directoryName.Length > 0) && (!Directory.Exists(directoryName))))
            { 
                Directory.CreateDirectory(directoryName); 
            } 
        }

        /// <summary>
        /// Checks if a MySettings file with a given name exist on disk
        /// </summary>
        /// <param name="settingsName">Name of the MySettings file</param>
        /// <returns></returns>
        public static bool DoesNameExistOnDisk(string settingsName)
        {
            try
            {            
                string filePath = GetFileName(settingsName);
                if (File.Exists(filePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks if a MySettings file with a given name exist on disk
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="settingsName">Name of the settings.</param>
        /// <returns></returns>
        public static bool DoesNameExistOnDisk(IUserContext userContext, string settingsName)
        {
            try
            {
                string filePath = GetFilePath(settingsName, userContext.User.UserName);
                if (File.Exists(filePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves a MySettings file to disk.
        /// </summary>
        /// <param name="settingsName">This will be the name of the settings file.</param>
        /// <param name="mySettings">MySettings object.</param>
        public static void SaveToDisk(string settingsName, MySettings mySettings)
        {
            if (string.IsNullOrEmpty(settingsName))
            {
                throw new Exception("name is empty");
            }

            if (mySettings.IsNull())
            {
                throw new Exception("MySettings object is null");
            }

            try
            {                
                string fileName = GetFileName(settingsName);
                //SaveFileToDisk(fileName, mySettings);
                SaveGZipFileToDisk(fileName, mySettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Saves a MySettings file to disk.
        /// The file will be stored in separate folder unique for the user.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="settingsName">The settings name.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public static void SaveToDisk(IUserContext userContext, string settingsName, MySettings mySettings)
        {
            if (string.IsNullOrEmpty(settingsName))
            {
                throw new Exception("Name is empty");
            }

            if (mySettings.IsNull())
            {
                throw new Exception("MySettings object is null");
            }

            if (userContext.IsNull() || userContext.User.IsNull() || string.IsNullOrEmpty(userContext.User.UserName))
            {
                throw new Exception("User object is not initialized");
            }

            string fileName = GetFilePath(settingsName, userContext.User.UserName);            
            SaveGZipFileToDisk(fileName, mySettings);
            //SaveFileToDisk(fileName, mySettings);
        }

        /// <summary>
        /// Saves a MySettings file to disk.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="mySettings">The MySettings object.</param>
        private static void SaveFileToDisk(string filePath, MySettings mySettings)
        {
            EnsureFolder(filePath);
            using (var writer = new FileStream(filePath, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(MySettings));
                ser.WriteObject(writer, mySettings);
            }
        }

        /// <summary>
        /// Saves a MySettings file to disk in GZip format.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="mySettings">The MySettings object.</param>
        private static void SaveGZipFileToDisk(string filePath, MySettings mySettings)
        {
            EnsureFolder(filePath);
            
            using (MemoryStream objectStream = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(MySettings));
                ser.WriteObject(objectStream, mySettings);                
                using (FileStream compressedFileStream = File.Create(filePath))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        compressionStream.Write(objectStream.ToArray(), 0, (int)objectStream.Length);
                    }             
                }
            }
        }

        /// <summary>
        /// Loads a MySettings file from disk.
        /// </summary>
        /// <param name="settingsName">The name of the MySettings file</param>
        /// <returns></returns>
        public static MySettings LoadFromDisk(string settingsName)
        {
            try
            {
                string filePath = GetFileName(settingsName);
                return LoadGZipFileFromDisk(filePath);
                //return LoadFileFromDisk(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Loads a MySettings file from disk.
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="settingsName">The name of the MySettings file</param>
        /// <returns></returns>
        public static MySettings LoadFromDisk(IUserContext userContext, string settingsName)
        {
            try
            {
                string fileName = GetFilePath(settingsName, userContext.User.UserName);
                return LoadGZipFileFromDisk(fileName);
                //return LoadFileFromDisk(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Loads a GZipped MySettings file file from disk.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        private static MySettings LoadGZipFileFromDisk(string filePath)
        {
            MySettings mySettings;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (var objectStream = new MemoryStream())
                    {
                        zipStream.CopyTo(objectStream);
                        objectStream.Position = 0;
                        using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(objectStream, new XmlDictionaryReaderQuotas()))
                        {
                            var ser = new DataContractSerializer(typeof(MySettings));
                            mySettings = (MySettings)ser.ReadObject(reader, true);
                            mySettings.Initialize();
                        }
                    }
                }                
            }
            return mySettings;
        }

        /// <summary>
        /// Returns a list with all saved setting files.
        /// </summary>        
        public static List<string> ListAllSavedSettingFiles()
        {
            var directory = new DirectoryInfo(GetSettingsServerPath());
            FileInfo[] files = directory.GetFiles("*" + FileExtension);
            var list = new List<string>();
            foreach (var fileInfo in files)
            {
                list.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
            }
            return list;
        }

        //public static MemoryStream Serialize(MySettings mySettings)
        //{
        //    var memoryStream = new MemoryStream();
        //    var ser = new DataContractSerializer(typeof(MySettings));            
        //    ser.WriteObject(memoryStream, mySettings);
        //    return memoryStream;
        //}

        public static void SaveLastSettings(IUserContext userContext, MySettings mySettings)
        {
            SaveToDisk(userContext, LastSettingsFileName, mySettings);
        }

        public static MySettings LoadLastSettings(IUserContext userContext)
        {
            return LoadFromDisk(userContext, LastSettingsFileName);            
        }

        public static MySettings LoadLastSettings(string userName)
        {
            string filePath = GetFilePath(LastSettingsFileName, userName);
            return LoadGZipFileFromDisk(filePath);
            //return LoadFromDisk(fileName);
        }

        public static bool DoesLastSettingsExist(IUserContext userContext)
        {
            string filePath = GetFilePath(LastSettingsFileName, userContext.User.UserName);
            return File.Exists(filePath);
        }

        public static void DeleteAllSettingsFiles(IUserContext userContext)
        {
            string directoryName = GetDirectoryPath(userContext.User.UserName);

            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, false);
            }
        }

        public static void DeleteLastSettingsFile(IUserContext userContext)
        {
            string filePath = GetFilePath(LastSettingsFileName, userContext.User.UserName);            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static DateTime? GetLastSettingsSaveTime(IUserContext userContext)
        {
            string filePath = GetFilePath(LastSettingsFileName, userContext.User.UserName);            
            if (File.Exists(filePath))
            {
                return File.GetLastWriteTime(filePath);
            }
            return null;
        }

        public static string GetLastSettingsSummary(IUserContext userContext)
        {
            return null;
        }

        /// <summary>
        /// Get all user uploaded mapdata files
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        public static string[] GetSavedMapDataFiles(IUserContext userContext)
        {
            var directory = GetMapDataDirectory(userContext);

            if (userContext.CurrentRole == null || !directory.Exists)
            {
                return null;
            }

            var files = directory.GetFiles();

            return (from f in files select f.Name.Remove(f.Name.LastIndexOf('.'))).ToArray();
        }

        /// <summary>
        /// Save uploaded file to disk
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static string SaveMapDataFile(IUserContext userContext, string fileName, Stream fileStream)
        {
            var filePath = InitFileSave(userContext, fileName, true);

            using (var compressedFile = File.Create(filePath))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);

                using (var output = new GZipStream(compressedFile, CompressionMode.Compress))
                {
                    output.Write(buffer, 0, buffer.Length);
                }

                compressedFile.Close();
            }

            return filePath;
        }

        /// <summary>
        /// Get a feture collection from file
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="fileName">File to get</param>
        /// <param name="coordinateSystemId">State returned coordinate system. None return data as is.</param>
        /// <returns></returns>
        public static FeatureCollection GetMapDataFeatureCollection(IUserContext userContext, string fileName, CoordinateSystemId coordinateSystemId)
        {
            var directory = GetMapDataDirectory(userContext);

            try
            {
                var decompressedStream =
                    new GZipStream(
                        new FileStream(GetMapDataFilePath(directory, fileName, true), FileMode.Open),
                        CompressionMode.Decompress);

                using (var streamReader = new StreamReader(decompressedStream))
                {
                    //Read json from file
                    var featureCollection = JsonConvert.DeserializeObject(
                        streamReader.ReadToEnd(), typeof(FeatureCollection)) as FeatureCollection;

                    streamReader.Close();

                    if (featureCollection == null)
                    {
                        return null;
                    }

                    var currentCoordinateSystem = GisTools.GeoJsonUtils.FindCoordinateSystem(featureCollection);

                    //Check if requested crs equals file crs
                    if (coordinateSystemId == CoordinateSystemId.None || currentCoordinateSystem.Id == coordinateSystemId)
                    {
                        return featureCollection;
                    }

                    //Convert coordinates before returning
                    var cm = new CoordinateConversionManager();
                    return cm.Convert(featureCollection, currentCoordinateSystem, new CoordinateSystem(coordinateSystemId));
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Detete uploaded file from disk
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="fileName">File to delete</param>
        /// <returns></returns>
        public static bool DeleteMapDataFile(IUserContext userContext, string fileName)
        {
            var directory = GetMapDataDirectory(userContext);

            if (!directory.Exists)
            {
                return false;
            }
            
            var file = new FileInfo(GetMapDataFilePath(directory, fileName, true));
            file.Delete();

            return true;
        }

        /// <summary>
        /// Check if file already exists
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool MapDataFileExists(IUserContext userContext, string fileName)
        {
            var directory = GetMapDataDirectory(userContext);
             
            return File.Exists(GetMapDataFilePath(directory, fileName, true));
        }

        public static string GetGeoJsonFromShapeZip(IUserContext userContext, string fileName, Stream fileStream)
        {
            //Get save path
            var zipFilePath = InitFileSave(userContext, fileName, false);

            //Save file to disk
            using (var zipFile = File.Create(zipFilePath))
            {
               fileStream.CopyTo(zipFile);
               zipFile.Close();

               fileStream.Close();
               fileStream.Dispose();
            }

            //Get temp directory for user
            var tempDirectory = GetTempDirectory(userContext);

            //Make sure temp direcory dosn't exists
            ForceDeleteDirectory(tempDirectory);

            //Extract files to temp dir
            ZipFile.ExtractToDirectory(zipFilePath, tempDirectory);
            
            //Delete zip file
            var zipfile = new FileInfo(zipFilePath);
            zipfile.Delete();
            
            //Get shape files from extracted files
            var shapeFiles = Directory.GetFiles(tempDirectory, "*.shp");

            if (shapeFiles.Length == 0)
            {
                throw new ImportGisFileException("No shapefile found in .zip-file. Please check that there are no subdirectories in the zip file you use. The data files must be located in the root.");
            }

            if (shapeFiles.Length != 1)
            {
                throw new ImportGisFileException("It should only be one shapefile in the .zip-file");                
            }
            var shapeFile = shapeFiles[0];
            string jsonString = null;
            using (var jsonStreamReader = new StreamReader(ShapeConverter.ConvertToGeoJsonStream(shapeFile)))
            {
                jsonString = jsonStreamReader.ReadToEnd();
                jsonStreamReader.Close();
            }

            //Clean Up, make sure temp direcory dosn't exists
            ForceDeleteDirectory(tempDirectory);

            return jsonString;
        }

        /// <summary>
        /// Get user map file directory
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static DirectoryInfo GetMapDataDirectory(IUserContext userContext)
        {
            var baseDirecory = GetDirectoryPath(userContext.User.UserName);

            return new DirectoryInfo(string.Format("{0}\\{1}", baseDirecory, MapDataDirectory));
        }

        private static string GetTempDirectory(IUserContext userContext)
        {
            return string.Format("{0}\\Temp", GetMapDataDirectory(userContext));
        }

        /// <summary>
        /// Create file path
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetMapDataFilePath(DirectoryInfo directory, string fileName, bool compress)
        {
            return string.Format("{0}\\{1}{2}", directory.FullName, fileName, compress ? ".gz" : "");
        }

        /// <summary>
        /// Forces deletion of directory even if the folder/containing files are write protected.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void ForceDeleteDirectory(string path)
        {
            DirectoryInfo root;
            Stack<DirectoryInfo> fols;
            DirectoryInfo fol;

            root = new DirectoryInfo(path);
            if (!root.Exists)
            {
                return;
            }
            fols = new Stack<DirectoryInfo>();
            fols.Push(root);
            while (fols.Count > 0)
            {
                fol = fols.Pop();                
                fol.Attributes = fol.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                foreach (DirectoryInfo d in fol.GetDirectories())
                {
                    fols.Push(d);
                }
                foreach (FileInfo f in fol.GetFiles())
                {
                    f.Attributes = f.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    f.Delete();
                }
            }
            root.Delete(true);
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="path"></param>
        private static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(path);
        }

        private static string InitFileSave(IUserContext userContext, string fileName, bool compress)
        {
            var directory = GetMapDataDirectory(userContext);

            if (!directory.Exists)
            {
                directory.Create();
            }
            return GetMapDataFilePath(directory, fileName, compress);
        }
    }
}
