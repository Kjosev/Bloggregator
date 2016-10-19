using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Article
{
    public class UpdateFavorite
    {
        public class Request : BaseRequest<Response>
        {
            public string Username { get; set; }
            public string ArticleId { get; set; }
        }

        public class Response : BaseResponse
        {
            public HttpStatusCode StatusCode { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                if (string.IsNullOrEmpty(request.Username))
                {
                    return new Response { StatusCode = HttpStatusCode.NotFound };
                }
                var allUsers = await Session.Query<Bloggregator.Domain.Entities.Account.User>().ToListAsync();
                var myUser = allUsers.Where(u => u.UserName == request.Username).FirstOrDefault();
                var me = await Session.LoadAsync<Bloggregator.Domain.Entities.Account.User>(myUser.Id);
                if (me.SavedArticleIds == null)
                {
                    me.SavedArticleIds = new List<string>();
                }
                if (me.SavedArticleIds.Contains(request.ArticleId))
                    me.SavedArticleIds.Remove(request.ArticleId);
                else
                    me.SavedArticleIds.Add(request.ArticleId);
                await Session.SaveChangesAsync();
                return new Response { StatusCode = HttpStatusCode.OK };
            }
        }
    }
}
