using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
/*
namespace AnalysisPortal.Helpers
{
    public class CssJsBuilder
    {
        public static void Initializing()
        {
            IBundleTransform jstransformer;
            IBundleTransform csstransformer;

#if DEBUG
            jstransformer = new NoTransform("text/javascript");
            csstransformer = new NoTransform("text/css");
#else
      jstransformer = new JsMinify();
      csstransformer = new CssMinify();
#endif

            var bundle = new Bundle("~/Scripts/js", jstransformer);

            bundle.AddFile("~/Scripts/jquery-1.6.2.js", true);
            bundle.AddFile("~/Scripts/jquery-ui-1.8.11.js", true);
            bundle.AddFile("~/Scripts/jquery.validate.unobtrusive.js", true);
            bundle.AddFile("~/Scripts/jquery.unobtrusive-ajax.js", true);
            bundle.AddFile("~/Scripts/jquery.validate.js", true);
            bundle.AddFile("~/Scripts/modernizr-2.0.6-development-only.js", true);
            bundle.AddFile("~/Scripts/AjaxLogin.js", true);
            bundle.AddFile("~/Scripts/bootstrap.js", true);
            bundle.AddFile("~/Scripts/dungeon.custom.js", true);

            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/css", csstransformer);

            bundle.AddFile("~/Content/bootstrap-responsive.css", true);
            bundle.AddFile("~/Content/bootstrap.css", true);

            BundleTable.Bundles.Add(bundle);

            bundle = new Bundle("~/Content/themes/base/css", csstransformer);

            bundle.AddFile("~/Content/themes/base/jquery.ui.core.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.resizable.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.selectable.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.accordion.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.autocomplete.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.button.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.dialog.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.slider.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.tabs.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.datepicker.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.progressbar.css", true);
            bundle.AddFile("~/Content/themes/base/jquery.ui.theme.css", true);

            BundleTable.Bundles.Add(bundle);
        }

    }
}
*/