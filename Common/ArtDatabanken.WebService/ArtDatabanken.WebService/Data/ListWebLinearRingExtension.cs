using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list
    /// of type WebLinearRing.
    /// </summary>
    public static class ListWebLinearRingExtension
    {
        /// <summary>
        /// Convert linears rings to JSON format.
        /// </summary>
        /// <param name="linearRings">Linears rings that should be converted.</param>
        /// <returns>Linears rings in JSON format.</returns>
        public static String GetJson(this List<WebLinearRing> linearRings)
        {
            Boolean firstLinearRing;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (linearRings.IsNotEmpty())
            {
                firstLinearRing = true;
                stringBuilder.Append("[");
                foreach (WebLinearRing linearRing in linearRings)
                {
                    if (firstLinearRing)
                    {
                        firstLinearRing = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(linearRing.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get linears rings as string.
        /// </summary>
        /// <param name="linearRings">Linears rings that should be converted.</param>
        /// <returns>Linears rings as string.</returns>
        public static String WebToString(this List<WebLinearRing> linearRings)
        {
            Boolean firstLinearRing;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (linearRings.IsNotEmpty())
            {
                firstLinearRing = true;
                stringBuilder.Append("Linear rings = [");
                foreach (WebLinearRing linearRing in linearRings)
                {
                    if (firstLinearRing)
                    {
                        firstLinearRing = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(linearRing.GetJson());
                }

                stringBuilder.Append("]");
            }

            return stringBuilder.ToString();
        }
    }
}
