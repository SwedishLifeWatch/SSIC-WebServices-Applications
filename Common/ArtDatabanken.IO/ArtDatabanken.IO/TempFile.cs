using System;
using System.IO;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class automatically creates a temporary file name.
    /// The file is deleted when the object is disposed.
    /// </summary>
    public class TempFile : IDisposable
    {
        private String _directoryPath;
        private String _fileName;

        /// <summary>
        /// Create a TempFile instance.
        /// </summary>
        public TempFile()
            : this(null)
        {
        }

        /// <summary>
        /// Create a TempFile instance.
        /// </summary>
        /// <param name='extension'>File extension for the temporary file.</param>
        public TempFile(String extension)
        {
            _directoryPath = Path.GetTempPath();
            _fileName = Path.GetRandomFileName();
            if (extension.IsNotEmpty())
            {
                _fileName = _fileName.Substring(0, _fileName.IndexOf(".") + 1) + extension;
            }
        }

        /// <summary>
        /// Get temporary directory.
        /// </summary>
        public String DirectoryPath
        {
            get { return _directoryPath; }
        }

        /// <summary>
        /// Get path for temporary file (directory path and file name).
        /// </summary>
        public String FilePath
        {
            get { return _directoryPath + _fileName; }
        }

        /// <summary>
        /// Get temporary file name (no directory).
        /// </summary>
        public String FileName
        {
            get { return _fileName; }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Deletes temporary file if it has been created.
        /// </summary>
        public void Dispose()
        {
            if (File.Exists(FilePath))
            {
                // Delete temporary file.
                // Set attribute to archive.
                // This avoids problem with read only files.
                File.SetAttributes(FilePath, FileAttributes.Archive);
                File.Delete(FilePath);
            }
        }
    }
}
