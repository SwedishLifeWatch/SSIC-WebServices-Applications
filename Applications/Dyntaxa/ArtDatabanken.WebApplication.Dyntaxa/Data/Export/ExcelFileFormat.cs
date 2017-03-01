namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public enum ExcelFileFormat
    {
        Excel2003,
        Excel2007,
        OpenXml
    }   

    public static class ExcelFileFormatHelper
    {
        public static readonly int Excel2003RowLimit = 64000;

        public static string GetExtension(ExcelFileFormat format)
        {
            switch (format)
            {
                case ExcelFileFormat.Excel2003:
                    return ".xls";
                case ExcelFileFormat.Excel2007:
                    return ".xlsx";
                case ExcelFileFormat.OpenXml:
                    return ".xlsx";
                default:
                    return ".xls";
            }
        }
    }
}
