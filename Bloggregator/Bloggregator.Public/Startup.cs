using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bloggregator.Public.Startup))]
namespace Bloggregator.Public
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
