using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Home;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Home
{
    /// <summary>
    /// Manager class for the Home controller
    /// </summary>
    public class HomeViewManager : ViewManagerBase
    {
        public HomeViewManager(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
            : base(userContext, mySettings)
        {
        }

        /// <summary>
        /// Creates the version number view model.
        /// </summary>
        /// <param name="assembly">The assembly to get build time from.</param>
        /// <returns></returns>
        public VersionNumberViewModel CreateVersionNumberViewModel(Assembly assembly)
        {
            var model = new VersionNumberViewModel();
            DateTime creationTime = GetBuildDateTime(assembly);
            model.CreationDate = creationTime.ToShortDateString() + " (" + creationTime.ToShortTimeString() + ")";
            model.Version = assembly.GetName().Version.ToString();
            model.ServerName = GetServerName();
            return model;
        }

        /// <summary>
        /// Returns the name of the server were the application is currently running
        /// </summary>
        /// <returns></returns>
        private string GetServerName()
        {
            string machineName = Environment.MachineName;
            return machineName;
        }

        /// <summary>
        /// Gets the build date time from an Assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        private DateTime GetBuildDateTime(Assembly assembly)
        {
            if (System.IO.File.Exists(assembly.Location))
            {
                var buffer = new byte[Math.Max(Marshal.SizeOf(typeof(ImageFileHeader)), 4)];
                using (var fileStream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Position = 0x3C;
                    fileStream.Read(buffer, 0, 4);
                    fileStream.Position = BitConverter.ToUInt32(buffer, 0); // COFF header offset
                    fileStream.Read(buffer, 0, 4); // "PE\0\0"
                    fileStream.Read(buffer, 0, buffer.Length);
                }
                var pinnedBuffer = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                try
                {
                    var coffHeader = (ImageFileHeader)Marshal.PtrToStructure(pinnedBuffer.AddrOfPinnedObject(), typeof(ImageFileHeader));

                    return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1) + new TimeSpan(coffHeader.TimeDateStamp * TimeSpan.TicksPerSecond));
                }
                finally
                {
                    pinnedBuffer.Free();
                }
            }
            return new DateTime();
        }

        struct ImageFileHeader
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }
    }
}
