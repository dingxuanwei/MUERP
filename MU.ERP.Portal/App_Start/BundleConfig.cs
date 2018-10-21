using System.Web;
using System.Web.Optimization;

namespace MU.ERP.Portal
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/vue").Include(
                        "~/Scripts/vue.js",
                        "~/Scripts/axios.js",
                        "~/Scripts/ElementUI/element-ui.js"));

            bundles.Add(new StyleBundle("~/Content/ElementUI").Include(
                      "~/Content/ElementUI/element-ui.css",
                      "~/Content/site.css"));
        }
    }
}
