using System.Web.Mvc;

namespace MU.ERP.Areas.Bas
{
    public class BasAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Bas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Bas_default",
                url: "Bas/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { string.Format("MU.ERP.Areas.{0}.Controllers", AreaName) }
            );
        }
    }
}