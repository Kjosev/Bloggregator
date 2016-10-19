using Bloggregator.AppServices.Pagination;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Admin.User
{
    public class List
    {
        public class Request : PaginationRequest<Response>
        {

        }
        
         public class Response : PaginationResponse
        {
            public List<User> Users { get; set; }
            public class User
            {
                public string Id { get; set; }
                public string UserName { get; set; }
                public string Email { get; set; }
                public bool IsActive { get; set; }
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var response = await Session.Query<Domain.Entities.Account.User>().ToListAsync();
                return new Response
                {
                    Users = response.Select(x => new Response.User
                    {
                        Email = x.Email,
                        Id = x.Id,
                        IsActive = x.IsActive,
                        UserName = x.UserName
                    }).ToList()
                };
            }
        }
    }
}
