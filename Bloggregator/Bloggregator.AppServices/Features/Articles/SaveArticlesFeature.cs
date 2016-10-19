using Bloggregator.Domain.Entities.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Articles
{
    public class SaveArticlesFeature
    {
        public class Request : BaseRequest<Response>
        {
            public List<Article> Articles { get; set; }
        }

        public class Response : BaseResponse
        {
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                foreach(var article in request.Articles)
                {
                    await Session.StoreAsync(article);
                    Console.WriteLine("Saved '{0}' to database", article.Title);
                }
                await Session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
