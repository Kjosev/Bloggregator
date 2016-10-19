using Bloggregator.Framework;
using StructureMap;

namespace Bloggregator.Public.DependencyReolution
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