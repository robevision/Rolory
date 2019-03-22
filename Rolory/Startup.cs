using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rolory.Startup))]
namespace Rolory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
