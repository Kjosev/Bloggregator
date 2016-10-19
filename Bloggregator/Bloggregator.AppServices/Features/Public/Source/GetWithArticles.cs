using Bloggregator.AppServices.Pagination;
using Bloggregator.AppServices.ViewModels;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Source
{
    public class GetWithArticles
    {
        public class Request : PaginationRequest<Response>
        {
            [Required]
            public string Id { get; set; }
            public string Username { get; set; }
            public string SearchTerm { get; set; }
        }

        public class Response : PaginationResponse
        {
            public string SearchTerm { get; set; }
            public SourceWithArticlesViewModel Source { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var source = await Session.LoadAsync<Domain.Entities.Sources.Source>(new Guid(request.Id));
                if (source == null)
                {
                    return new Response();
                }
                var articles = await Session.Query<Domain.Entities.Articles.Article>().ToListAsync();
                articles = articles.OrderByDescending(x => x.UpdatedDate).Where(x => x.SourceId == request.Id).ToList();
                var articlesVm = new List<ArticleViewmodel>();
                
                foreach (var article in articles)
                {
                    var articlevm = new ArticleViewmodel(article);
                    articlesVm.Add(articlevm);
                }
                
                if (!string.IsNullOrEmpty(request.Username))
                {
                    var allUsers = await Session.Query<Bloggregator.Domain.Entities.Account.User>().ToListAsync();
                    var user = allUsers.Where(x => x.UserName == request.Username).FirstOrDefault();
                    if (user != null)
                    {
                        articlesVm.Where(a => user.SavedArticleIds.Contains(a.Id)).ToList().ForEach(a => a.Favorite = true);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var toReturnFiltered = new List<ArticleViewmodel>();
                    foreach (var article in articlesVm)
                    {
                        if (article.Description.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.Content.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.Title.ToLower().Contains(request.SearchTerm.ToLower())
                            || article.SourceName.ToLower().Contains(request.SearchTerm.ToLower()))
                        {
                            toReturnFiltered.Add(article);
                        }
                    }
                    articlesVm = toReturnFiltered;
                }

                var total = articlesVm.Count;
                articlesVm =  articlesVm.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();
                
                return new Response
                {
                    Source = new SourceWithArticlesViewModel { Source = source, Articles = articlesVm },
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItemCount = total
                };
            }
        }
    }
}
