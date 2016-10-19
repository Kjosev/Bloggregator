using Bloggregator.Framework;
using StructureMap;

namespace Bloggregator.Admin.DependencyReolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var bootstrapper = new Bootstrapper(new BootstrapperSettings());
            bootstrapper.Container.Configure(c => c.AddRegistry<DefaultRegistry>());
            return bootstrapper.Container;
        }
    }
}