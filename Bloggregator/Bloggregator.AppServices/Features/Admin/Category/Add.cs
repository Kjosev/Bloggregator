using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;

namespace Bloggregator.AppServices.Features.Admin.Category
{
    public class Add
    {
        public class Request : BaseRequest<Response>
        {
            [Required]
            public string Name { get; set; }
        }
        
        public class Response : BaseResponse
        {
            public Guid CategoryId { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var createdCategory = new Domain.Entities.Categories.Category()
                {
                    Name = request.Name
                };

                await Session.StoreAsync(createdCategory);

                await Session.SaveChangesAsync();

                return new Response()
                {
                    CategoryId = createdCategory.Id
                };
            }
        }
    }
}
