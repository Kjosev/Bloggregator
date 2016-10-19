using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Account
{
    public class UpdatePermissions
    {
        public class Request : BaseRequest<Response>
        {
            public string Username { get; set; }
        }

        public class Response : BaseResponse
        {

        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var categories = await Session.Query<Bloggregator.Domain.Entities.Categories.Category>().ToListAsync();
                var sources = await Session.Query<Bloggregator.Domain.Entities.Sources.Source>().ToListAsync();
                foreach(var category in categories)
                {
                    var categoryPermision = new Bloggregator.Domain.Entities.Categories.CategoryPermission();
                    categoryPermision.Username = request.Username;
                    categoryPermision.Visible = true;
                    categoryPermision.CategoryName = category.Name;
                    categoryPermision.CategoryId = category.Id.ToString();
                    categoryPermision.Sources = new List<Bloggregator.Domain.Entities.Sources.SourcePermission>();
                    foreach (var sourceId in category.SourceIds)
                    {
                        var source = sources.Where(x => x.Id.ToString() == sourceId).FirstOrDefault();
                        var sourcePermission = new Bloggregator.Domain.Entities.Sources.SourcePermission();
                        sourcePermission.SourceId = source.Id.ToString();
                        sourcePermission.SourceName = source.Name;
                        sourcePermission.SourceUrl = source.Url;
                        sourcePermission.Visible = true;
                        categoryPermision.Sources.Add(sourcePermission);
                    }
                    await Session.StoreAsync(categoryPermision);
                }
                await Session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
