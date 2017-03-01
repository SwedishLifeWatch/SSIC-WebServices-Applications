using System.IO;
using System.Web.Hosting;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This manager class handles file system operations and generating filenames.
    /// </summary>
    public static class FileSystemManager
    {
        /// <summary>
        /// Ensures that the folder on disk exists.
        /// If it doesn't exist, the folder will be created.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void EnsureFolderExists(string path)
        {
            path = MapPath(path);
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        /// <summary>
        /// Delete all files in the specified path that is matching the search pattern.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern. For example "*.zip".</param>
        public static void DeleteAllFiles(string path, string searchPattern)
        {
            path = MapPath(path);
            string[] files = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Delete all files in the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void DeleteAllFiles(string path)
        {
            path = MapPath(path);
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Creates a random filename.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>A valid random filename.</returns>
        public static string CreateRandomFilename(string fileExtension)
        {
            string filename = Path.GetRandomFileName();
            filename = Path.ChangeExtension(filename, fileExtension);
            filename = GetValidFilename(filename);
            return filename;
        }

        /// <summary>
        /// Creates a valid random filename with a specific prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>A valid random filename.</returns>
        public static string CreateRandomFilenameWithPrefix(string prefix, string fileExtension)
        {
            if (prefix == null)
            {
                prefix = string.Empty;
            }

            string filename = string.Format("{0}{1}", prefix, Path.GetRandomFileName());
            filename = Path.ChangeExtension(filename, fileExtension);
            filename = GetValidFilename(filename);
            return filename;
        }

        /// <summary>
        /// Creates a random filename with suffix.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>A valid random filename.</returns>
        public static string CreateRandomFilenameWithSuffix(string suffix, string fileExtension)
        {
            if (suffix == null)
            {
                suffix = string.Empty;
            }

            string filename = Path.GetRandomFileName();
            filename = string.Format("{0}{1}", Path.GetFileNameWithoutExtension(filename), suffix);
            filename = Path.ChangeExtension(filename, fileExtension);
            filename = GetValidFilename(filename);
            return filename;
        }

        /// <summary>
        /// Gets a valid filename by removing illegal filename characters and
        /// removing duplicate blanks.
        /// </summary>
        /// <param name="suggestedFilename">The suggested filename.</param>
        /// <returns>A valid filename.</returns>
        public static string GetValidFilename(string suggestedFilename)
        {
            string filename = suggestedFilename.Trim().RemoveDuplicateBlanks();
            filename = RemoveIllegalFileNameCharacters(filename);
            return filename;
        }

        /// <summary>
        /// Replaces all illegal file name characters with '_'
        /// and returns a legal file name.
        /// </summary>
        /// <param name="filename">The suggested filename.</param>
        /// <returns>A valid filename.</returns>
        private static string RemoveIllegalFileNameCharacters(string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return filename;
        }        

        /// <summary>
        /// Gets the file path on server by combining path and filename and look up the physical location.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>An absolute file path.</returns>
        public static string GetAbsoluteFilePath(string path, string filename)
        {
            return MapPath(Path.Combine(path, filename));            
        }

        /// <summary>
        /// Combines the path and filename.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filename">The filename.</param>
        /// <returns>A file path.</returns>
        public static string CombinePathAndFilename(string path, string filename)
        {
            return Path.Combine(path, filename);
        }

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A mapped path.</returns>
        public static string MapPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            path = HostingEnvironment.MapPath(path);
            return path;
        }       
    }
}
