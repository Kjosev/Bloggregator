using Bloggregator.AppServices.Authorization;
using Bloggregator.Domain.Entities.Account;
using Bloggregator.Framework.Identity;
using Bloggregator.Public.Models;
using Microsoft.AspNet.Identity;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Bloggregator.Public.DependencyReolution
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


            For<IUserStore<User>>().Use("Create Store", ctx => new RavenUserStore<User>(ctx.GetInstance<IAsyncDocumentSession>(), false));
            For<UserManager<User>>().Use("Create User manager", ctx => new BloggregatorUserManager(ctx.GetInstance<IUserStore<User>>()));
        }
    }
}