using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MU.ERP.SSO.Startup))]
namespace MU.ERP.SSO
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
