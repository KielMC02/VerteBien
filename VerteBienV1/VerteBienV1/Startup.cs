using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VerteBienV1.Startup))]
namespace VerteBienV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
