using Bloggregator.AppServices.Pagination;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Article
{
    public class GetFavourites
    {
        public class Request : PaginationRequest<Response>
        {
            public string SearchTerm { get; set; }
            public string Username { get; set; }
        }

        public class Response : PaginationResponse
        {
            public string SearchTerm { get; set; }
            public IList<Bloggregator.Domain.Entities.Articles.Article> Articles { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                if (string.IsNullOrEmpty(request.Username))
                {
                    return new Response();
                }
                var allUsers = await Session.Query<Bloggregator.Domain.Entities.Account.User>().ToListAsync();
                var me = allUsers.Where(u => u.UserName == request.Username).FirstOrDefault();
                var allArticles = await Session.Query<Bloggregator.Domain.Entities.Articles.Article>().ToListAsync();
                if (me.SavedArticleIds == null)
                {
                    return new Response { Articles = new List<Bloggregator.Domain.Entities.Articles.Article>() };
                }
                var favoriteArticles = allArticles.Where(a => me.SavedArticleIds.Contains(a.Id.ToString())).ToList();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var toReturnFiltered = new List<Bloggregator.Domain.Entities.Articles.Article>();
                    foreach (var article in favoriteArticles)
                    {
                        if (article.Description.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.Content.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.Title.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.SourceName.ToLower().Contains(request.SearchTerm.ToLower()))
                        {
                            toReturnFiltered.Add(article);
                        }
                    }
                    favoriteArticles = toReturnFiltered;
                }

                var total = favoriteArticles.Count;
                favoriteArticles = favoriteArticles.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

                return new Response
                {
                    Articles = favoriteArticles,
                    TotalItemCount = total,
                    Page = request.Page,
                    PageSize = request.PageSize
                };
            }
        }
    }
}
