using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Bloggregator.AppServices.Features.Admin.Category;
using Raven.Client;

namespace Bloggregator.AppServices.Features.Admin.Source
{
    public class Add
    {
        public class Request : BaseRequest<Response>
        {
            [Required]
            public string Name { get; set; }

            [Required]
            [Url]
            public string Url { get; set; }

            [Required(ErrorMessage = "You must select a category!")]
            public Guid CategoryId { get; set; }
        }

        public class Response : BaseResponse
        {
            public Guid SourceId { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var sourceCategory = await Session.LoadAsync<Domain.Entities.Categories.Category>(request.CategoryId);

                if(sourceCategory == null)
                {
                    return new Response();
                }

                
                var createdSource = new Domain.Entities.Sources.Source()
                {
                    Name = request.Name,
                    Url = request.Url,
                    CategoryId = sourceCategory.Id
                };

                sourceCategory.SourceIds.Add(createdSource.Id.ToString());

                await Session.StoreAsync(createdSource);

                await Session.SaveChangesAsync();

                return new Response()
                {
                    SourceId = createdSource.Id
                };
            }
        }
    }
}
