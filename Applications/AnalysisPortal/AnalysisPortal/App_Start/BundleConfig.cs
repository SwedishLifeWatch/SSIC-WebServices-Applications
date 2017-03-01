using System.Web;
using System.Web.Optimization;

namespace AnalysisPortal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
            BundleTable.EnableOptimizations = true;            
#endif
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/AnalysisPortal.css",
                "~/Content/animate.min.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/bootstrap.slider.css",
                "~/Content/bootstrap.treeview.min.css",
                "~/Content/ext-all-slate-min.css",
                "~/Content/font-awesome.css",
                "~/Content/jquery.fileupload.css",
                "~/Content/jquery.miniColors.css",
                "~/Content/map.css",
                "~/Content/Overrides.css",
                "~/Content/select2.css",
                "~/Content/toastr.css",
                "~/Content/jquery.liScroll.css",
                "~/Content/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/extjs").Include(
                        "~/Scripts/extjs-4.2.1/ext-all.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            bundles.Add(new ScriptBundle("~/bundles/AnalysisPortal").Include(
                            "~/Scripts/AnalysisPortal/AnalysisPortal.js",
                            "~/Scripts/AnalysisPortal/AnalysisPortal.Models.js",
                            "~/Scripts/AnalysisPortal/AnalysisPortal.GIS.js",
                            "~/Scripts/AnalysisPortal/AnalysisPortal.WFS.Formula.js",
                            "~/Scripts/AnalysisPortal/AnalysisPortal.WFS.js",
                            "~/Scripts/AnalysisPortal/AnalysisPortal.Statistics.js",
                            "~/Scripts/AnalysisPortal/ExtJsPlugins.js"));          
                        
            bundles.Add(new ScriptBundle("~/bundles/jquery-plugins").Include(
                "~/Scripts/jquery-2.1.4.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.subcookie.js",
                "~/Scripts/select2/select2.js",
                "~/Scripts/toastr/toastr.js",
                "~/Scripts/jquery-miniColors/jquery.miniColors.js",
                "~/Scripts/json2.js",
                "~/Scripts/jquery.urldecoder.js",
                "~/Scripts/jquery.detectmobilebrowser.js",
                "~/Scripts/jQuery-File-Upload/js/vendor/jquery.ui.widget.js",
                "~/Scripts/jQuery-File-Upload/js/jquery.iframe-transport.js",
                "~/Scripts/jQuery-File-Upload/js/jquery.fileupload.js",
                "~/Scripts/jquery.dad.js",
                "~/Scripts/jquery.liScroll.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap*",
                "~/Scripts/Bootstrap-datepicker/bootstrap-datepicker.js"));
                        
            bundles.Add(new ScriptBundle("~/bundles/OpenLayersUtils").Include(  
                "~/Scripts/OpenLayers/OpenLayers.js",
                "~/Scripts/OpenLayers/Lang/sv-SE.js",
                "~/Scripts/OpenLayers/Lang/en.js",
                "~/Scripts/proj4js/proj4js-combined.js"));
        }
    }
}