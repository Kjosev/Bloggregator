using Bloggregator.AppServices.Authorization;
using Bloggregator.Admin.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;
using Bloggregator.Domain.Entities.Account;

namespace Bloggregator.Admin.DependencyReolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });


            For<IUserStore<Administrator>>().Use("Create Store", ctx => new RavenUserStore<Administrator>(ctx.GetInstance<IAsyncDocumentSession>(), false));
            For<UserManager<Administrator>>().Use("Create User manager", ctx => new UserManager<Administrator>(ctx.GetInstance<IUserStore<Administrator>>()));
        }
    }
}