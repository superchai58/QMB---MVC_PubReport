using System.Web;
using System.Web.Optimization;
using System.IO;

namespace MVC_PubReport
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/jquery.js").Include(
                 "~/Scripts/jquery-3.3.1.js",
                 "~/Scripts/bootstrap.js",
                 "~/Scripts/bootstrap-table.js",
                 "~/Scripts/bootstrap-table-editable.js",
                  "~/Scripts/bootstrap-editable.js",
                   "~/Scripts/tableExport.js",
                   "~/Scripts/jquery.combo.select.js",
                 "~/Scripts/jq22.js"
               
                ));



            bundles.Add(new StyleBundle("~/CSS.css").Include(
                    "~/CSS/bootstrap.css",
                     "~/CSS/bootstrap-table.css",
                     "~/CSS/bootstrap-editable.css",
                    "~/CSS/site.css",
                     "~/CSS/head.css",
                    "~/CSS/jq22.css",
                     "~/CSS/combo.select.css",
                    "~/CSS/thickbox.css"
                ));
            bundles.Add(new ScriptBundle("~/Pub_Report.js").Include(
                "~/Scripts/QMSSDK.js",
                 "~/Scripts/thickbox.js",
                "~/Scripts/Pub_Report.js"


                ));
         
        }
    }
}