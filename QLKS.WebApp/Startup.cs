using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLKS.WebApp.Startup))]
namespace QLKS.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
