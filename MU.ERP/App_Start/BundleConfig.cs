using System.Web;
using System.Web.Optimization;

namespace MU.ERP
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            ResetIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                        "~/Content/easyui/jquery.easyui.min.js"));

            bundles.Add(new StyleBundle("~/Content/easyuicss").Include(
                      "~/Content/easyui/themes/default/easyui.css",
                      "~/Content/easyui/themes/color.css",
                      "~/Content/easyui/themes/icon.css"));
        }

        public static void ResetIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList.Clear();
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}
