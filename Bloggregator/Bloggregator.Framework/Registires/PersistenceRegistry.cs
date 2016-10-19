using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Framework.Registires
{
    public class PersistenceRegistry : Registry
    {
        public PersistenceRegistry(AbstractBootstrapperSettings settings)
        {
            ForSingletonOf<IDocumentStore>().Use("Build the DocumentStore", ctx =>
            {
                var store = new DocumentStore()
                {
                    Url = settings.DatabaseUrl,
                    DefaultDatabase = settings.DatabaseName
                }.Initialize();

                return store;
            });

            For<IDocumentSession>().Use("Open session", ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
            For<IAsyncDocumentSession>().Use("Open async session", ctx => CreateAsyncSession(ctx)).AlwaysUnique();
        }

        private IAsyncDocumentSession CreateAsyncSession(IContext ctx)
        {
            var session = ctx.GetInstance<IDocumentStore>().OpenAsyncSession();
            session.Advanced.UseOptimisticConcurrency = true;

            return session;
        }
    }
}
