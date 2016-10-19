using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Admin.User
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
            public string Id { get; set; }
            
            [Required]
            public bool IsActive { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var user = await Session.LoadAsync<Domain.Entities.Account.User>(request.Id);
                
                user.IsActive = request.IsActive;

                await Session.SaveChangesAsync();

                return new Response()
                {
                    IsSuccessful = true
                };
            }
        }
    }
}
