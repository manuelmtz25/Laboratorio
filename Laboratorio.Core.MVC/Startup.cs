using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Laboratorio.Core.MVC.Startup))]
namespace Laboratorio.Core.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
