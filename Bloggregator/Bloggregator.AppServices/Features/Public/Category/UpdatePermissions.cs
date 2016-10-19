using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace Bloggregator.AppServices.Features.Public.Category
{
    public class UpdatePermissions
    {
        public class Request : BaseRequest<Response>
        {
            public IList<Bloggregator.Domain.Entities.Categories.CategoryPermission> Categories { get; set; }
        }

        public class Response : BaseResponse
        {
            public IList<Bloggregator.Domain.Entities.Categories.CategoryPermission> Categories { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                foreach(var categoryPermission in request.Categories)
                {
                    var entity = await Session.LoadAsync<Bloggregator.Domain.Entities.Categories.CategoryPermission>(categoryPermission.Id);
                    entity.Visible = categoryPermission.Visible;
                    entity.Sources = categoryPermission.Sources;
                }
                await Session.SaveChangesAsync();

                return new Response { Categories = request.Categories };
            }
        }
    }
}
