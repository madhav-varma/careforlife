using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CareForLife.Startup))]
namespace CareForLife
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
