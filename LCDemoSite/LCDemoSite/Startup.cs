using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LCDemoSite.Startup))]
namespace LCDemoSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
