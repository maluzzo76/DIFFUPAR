using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASDWP.Startup))]
namespace ASDWP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
