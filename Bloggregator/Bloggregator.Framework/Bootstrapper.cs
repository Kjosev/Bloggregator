using Bloggregator.Framework.Registires;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Framework
{
    public class Bootstrapper
    {
        public IContainer Container { get; set; }

        public Bootstrapper(AbstractBootstrapperSettings settings)
        {
            Container = new Container(cfg =>
            {
                cfg.AddRegistry(new MediatorRegistry());
                cfg.AddRegistry(new PersistenceRegistry(settings));
            });

            var serviceLocationProvider = new ServiceLocatorProvider(
               () => new StructureMapServiceLocator(Container));
            ServiceLocator.SetLocatorProvider(serviceLocationProvider);

            string whatdidiscan = Container.WhatDidIScan();
            string whatdoihave = Container.WhatDoIHave();
        }
    }
}
