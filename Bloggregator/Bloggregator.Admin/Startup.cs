using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bloggregator.Admin.Startup))]
namespace Bloggregator.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
