using Raven.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Admin.Category
{
    public class Edit
    {
        public class Response : BaseResponse
        {
            public bool IsSuccessful { get; set; }
        }

        public class Request : BaseRequest<Response>
        {
            [Required]
            public Guid CategoryId { get; set; }

            /// <summary>
            /// Enable category if true, else disable
            /// </summary>
            [Required]
            public bool IsActive { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var category = await Session.LoadAsync<Domain.Entities.Categories.Category>(request.CategoryId);

                // Disable all sources of this category
                //var sources = await Session.Query<Domain.Entities.Sources.Source>()
                //    .Where(s => s.CategoryId == request.CategoryId)
                //    .ToListAsync();

                //foreach (var source in sources)
                //{
                //    source.IsActive = request.IsActive;
                //}

                category.IsActive = request.IsActive;

                await Session.SaveChangesAsync();

                return new Response()
                {
                    IsSuccessful = true
                };
            }
        }
    }
}
