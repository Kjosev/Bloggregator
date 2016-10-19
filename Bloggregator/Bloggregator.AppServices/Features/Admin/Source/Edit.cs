using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Admin.Source
{
    public class Edit
    {
        public class Request : BaseRequest<Response>
        {
            [Required]
            public Guid SourceId { get; set; }

            /// <summary>
            /// Enable category if true, else disable
            /// </summary>
            [Required]
            public bool IsActive { get; set; }
        }

        public class Response : BaseResponse
        {
            public bool IsSuccessful { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var source =
                    await Session.LoadAsync<Domain.Entities.Sources.Source>(request.SourceId);

                // Check if category is active first
                var sourceCategory
                    = await Session.LoadAsync<Domain.Entities.Categories.Category>(source.CategoryId);

                if (!sourceCategory.IsActive)
                {
                    return new Response()
                    {
                        IsSuccessful = false
                    };
                }

                source.IsActive = request.IsActive;
                await Session.SaveChangesAsync();

                return new Response()
                {
                    IsSuccessful = true
                };
            }
        }
    }
}
