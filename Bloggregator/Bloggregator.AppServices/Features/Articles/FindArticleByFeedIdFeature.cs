using Bloggregator.Domain.Entities.Articles;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Articles
{
    public class FindArticleByFeedIdFeature
    {
        public class Request : BaseRequest<Response>
        {
            public string FeedId { get; set; }
        }

        public class Response : BaseResponse
        {
            public bool Exists { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var exists = await Session.Query<Article>().AnyAsync(a => a.UrlThroughFeed == request.FeedId);

                return new Response { Exists = exists };
            }
        }
    }
}
