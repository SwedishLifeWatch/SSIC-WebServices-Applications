using System.IO;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers
{
    /// <summary>
    /// This class is used to copy MySettings objects or 
    /// objects inside MySettings that inherits SettingBase
    /// </summary>
    //public static class MySettingsCopier
    //{
    //    public static T Clone<T>(T source) where T : SettingBase
    //    {
    //        T obj = ObjectCopier.Clone(source);
    //        obj.EnsureInitialized();
    //        return obj;
    //    }
    //}

    /// <summary>
    /// This class is used to copy objects
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Clones an object using DataContract serializing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            //if (!Attribute.IsDefined(typeof(T), typeof(DataContractAttribute)))
            //{
            //    throw new ArgumentException("The type must be serializable.", "source");
            //}

            using (Stream stream = new MemoryStream())
            {
                var ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)ser.ReadObject(stream);
            }
        }
    }
}
