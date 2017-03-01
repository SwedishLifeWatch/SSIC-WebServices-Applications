using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.IO.Test
{
    [TestClass]
    public class TempFileTest
    {
        public TempFileTest()
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void Constructor()
        {
            String extension;

            using (TempFile tempFile = new TempFile())
            {
                Assert.IsNotNull(tempFile);
                Assert.IsTrue(tempFile.DirectoryPath.IsNotEmpty());
                Assert.IsTrue(tempFile.FileName.IsNotEmpty());
                Assert.IsTrue(tempFile.FilePath.IsNotEmpty());
                Assert.AreEqual(tempFile.FilePath, tempFile.DirectoryPath + tempFile.FileName);
            }

            extension = "txt";
            using (TempFile tempFile = new TempFile(extension))
            {
                Assert.IsNotNull(tempFile);
                Assert.IsTrue(tempFile.DirectoryPath.IsNotEmpty());
                Assert.IsTrue(tempFile.FileName.IsNotEmpty());
                Assert.IsTrue(tempFile.FilePath.IsNotEmpty());
                Assert.AreEqual(tempFile.FilePath, tempFile.DirectoryPath + tempFile.FileName);
                Assert.AreEqual(extension, tempFile.FileName.Substring(tempFile.FileName.IndexOf(".") + 1));
            }
        }

        [TestMethod]
        public void DirectoryPath()
        {
            using (TempFile tempFile = new TempFile())
            {
                Assert.IsTrue(tempFile.DirectoryPath.IsNotEmpty());
            }
        }

        [TestMethod]
        public void Dispose()
        {
            FileStream fileStream;
            String extension;
            String filePath;

            // Test with no file.
            extension = "txt";
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
            }
            Assert.IsFalse(File.Exists(filePath));
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
                tempFile.Dispose();
            }
            Assert.IsFalse(File.Exists(filePath));

            // Test with existing file.
            extension = "txt";
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
                fileStream = File.OpenWrite(filePath);
                fileStream.WriteByte(48);
                fileStream.Close();
            }
            Assert.IsFalse(File.Exists(filePath));
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
                fileStream = File.OpenWrite(filePath);
                fileStream.WriteByte(48);
                fileStream.Close();
                tempFile.Dispose();
            }
            Assert.IsFalse(File.Exists(filePath));

            // Test with existing read only file.
            extension = "txt";
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
                fileStream = File.OpenWrite(filePath);
                fileStream.WriteByte(48);
                fileStream.Close();
                File.SetAttributes(filePath, FileAttributes.ReadOnly);
            }
            Assert.IsFalse(File.Exists(filePath));
            using (TempFile tempFile = new TempFile(extension))
            {
                filePath = tempFile.FilePath;
                fileStream = File.OpenWrite(filePath);
                fileStream.WriteByte(48);
                fileStream.Close();
                File.SetAttributes(filePath, FileAttributes.ReadOnly);
                tempFile.Dispose();
            }
            Assert.IsFalse(File.Exists(filePath));
        }

        [TestMethod]
        public void FilePath()
        {
            using (TempFile tempFile = new TempFile())
            {
                Assert.IsTrue(tempFile.FilePath.IsNotEmpty());
                Assert.AreEqual(tempFile.FilePath, tempFile.DirectoryPath + tempFile.FileName);
            }
        }

        [TestMethod]
        public void FileName()
        {
            using (TempFile tempFile = new TempFile())
            {
                Assert.IsTrue(tempFile.FileName.IsNotEmpty());
            }
        }
    }
}
