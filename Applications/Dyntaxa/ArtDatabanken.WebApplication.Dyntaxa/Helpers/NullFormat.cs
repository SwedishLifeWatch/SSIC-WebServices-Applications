using System;

namespace ArtDatabanken.WebApplication.Dyntaxa.Helpers
{
    /// <summary>
    /// Handles null formatting in String.Format(...)
    /// </summary>
    public class NullFormat : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public object GetFormat(Type service)
        {
            if (service == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Formats the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg">The argument.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public string Format(string format, object arg, IFormatProvider provider)
        {
            string val = arg as string;
            if (arg == null || String.IsNullOrEmpty(val))
            {
                return "\"\"";                
            }
            
            IFormattable formattable = arg as IFormattable;
            if (formattable != null)
            {
                return formattable.ToString(format, provider);
            }
            return arg.ToString();
        }
    }
}