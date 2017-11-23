using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ApbBackend.Startup))]

namespace ApbBackend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}