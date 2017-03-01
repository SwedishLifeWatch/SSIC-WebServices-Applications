using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to a list of Int32 instances.
    /// </summary>
    public static class ListInt32Extension
    {
        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified search criteria.
        /// </summary>
        /// <param name="integers">A list of integers.</param>
        /// <param name="integerColumn">Name of column with integer value.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this List<Int32> integers,
                                               String integerColumn)
        {
            Boolean isFirstItem;
            StringBuilder whereCondition;

            whereCondition = new StringBuilder();
            if (integers.IsNotEmpty())
            {
                if (integers.Count == 1)
                {
                    whereCondition.Append("(" + integerColumn + " = ");
                    whereCondition.Append(integers[0]);
                    whereCondition.Append(")");
                }
                else
                {
                    isFirstItem = true;
                    whereCondition.Append("(" + integerColumn + " IN (");
                    foreach (Int32 integer in integers)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            whereCondition.Append(", ");
                        }

                        whereCondition.Append(integer);
                    }

                    whereCondition.Append("))");
                }
            }

            return whereCondition.ToString();
        }

        /// <summary>
        /// Get a list of integers as a string.
        /// </summary>
        /// <param name="integers">A list of integers.</param>
        /// <returns>A list of integers as a string.</returns>
        public static String WebToString(this List<Int32> integers)
        {
            Boolean isFirstItem;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (integers.IsNotEmpty())
            {
                if (integers.Count == 1)
                {
                    stringBuilder.Append(integers[0]);
                }
                else
                {
                    isFirstItem = true;
                    stringBuilder.Append("[");
                    foreach (Int32 number in integers)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            stringBuilder.Append(", ");
                        }

                        stringBuilder.Append(number);
                    }

                    stringBuilder.Append("]");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
