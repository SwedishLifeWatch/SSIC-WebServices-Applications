using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.AnalysisPortal.Result;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettingsSummary.Improved2
{
    //public abstract class ImprovedMySettingsSummaryBase2
    //{
    //    public abstract ImprovedMySettingsSummaryItemBase2 ShortData { get; }
    //    public abstract ImprovedMySettingsSummaryItemBase2 LargeData { get; }
    //}

    ////public abstract class ImprovedMySettingsSummaryItemBase2
    ////{
    ////    public abstract ImprovedMySettingsSummaryItemType Type { get; }        
    ////}

    //public interface ImprovedMySettingsSummaryItemBase2
    //{
    //    ImprovedMySettingsSummaryItemType Type { get; }
    //    List<ResultType> AffectedResultTypes { get; }
    //}

    //public enum ImprovedMySettingsSummaryItemType
    //{
    //    Table,
    //    Hierarchical,
    //    List
    //}

    //public class MySettingsSummaryTable : DataTable, ImprovedMySettingsSummaryItemBase2
    //{
    //    //public override ImprovedMySettingsSummaryItemType Type { get { return ImprovedMySettingsSummaryItemType.Table; } }
    //    //public DataTable Table { get; set; }
    //    public ImprovedMySettingsSummaryItemType Type
    //    {
    //        get
    //        {
    //            return ImprovedMySettingsSummaryItemType.Table;
    //        }
    //    }

    //    public List<ResultType> AffectedResultTypes { get; set; }
    //}

    //public class MySettingsSummaryList : List<string>, ImprovedMySettingsSummaryItemBase2
    //{
    //    //public override ImprovedMySettingsSummaryItemType Type { get { return ImprovedMySettingsSummaryItemType.Table; } }
    //    //public DataTable Table { get; set; }
    //    public ImprovedMySettingsSummaryItemType Type
    //    {
    //        get
    //        {
    //            return ImprovedMySettingsSummaryItemType.List;
    //        }
    //    }
    //    public List<ResultType> AffectedResultTypes { get; set; }
    //}

    //public class MySettingsSummaryHierarchical : ImprovedMySettingsSummaryItemBase2
    //{
    //    public ImprovedMySettingsSummaryItemType Type
    //    {
    //        get
    //        {
    //            return ImprovedMySettingsSummaryItemType.Hierarchical;
    //        }
    //    }

    //    public List<MySettingsSummaryHierarchicalGroup> Groups { get; set; }
    //    public List<ResultType> AffectedResultTypes { get; set; }
    //}

    //public class MySettingsSummaryHierarchicalGroup
    //{
    //    public string Title { get; set; }
    //    public List<string> Items { get; set; }
    //    public ImprovedMySettingsSummaryItemBase2 Items2 { get; set; }
    //}

    ////public class MySettingsSummaryTable : ImprovedMySettingsSummaryItemBase2
    ////{
    ////    public override ImprovedMySettingsSummaryItemType Type { get { return  ImprovedMySettingsSummaryItemType.Table;} }
    ////    public DataTable Table { get; set; }
    ////}
}