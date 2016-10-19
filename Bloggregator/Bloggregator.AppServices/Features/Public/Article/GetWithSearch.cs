using Bloggregator.AppServices.Pagination;
using Bloggregator.AppServices.ViewModels;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Article
{
    public class GetWithSearch
    {

        public class Request : PaginationRequest<Response>
        {
            public string Username { get; set; }
            public string SearchTerm { get; set; }
        }

        public class Response : PaginationResponse
        {
            public string SearchTerm { get; set; }
            public List<ArticleViewmodel> Articles { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var allSources = await Session.Query<Domain.Entities.Sources.Source>().ToListAsync();
                // if no user logged in, every source should be visible
                var visibleSources = allSources.Select(s => s.Id.ToString()).ToList();

                if (!string.IsNullOrEmpty(request.Username))
                {
                    // get user preferences for all categories
                    var categoryPermissions = await Session.Query<Domain.Entities.Categories.CategoryPermission>()
                        .Where(x => x.Username == request.Username && x.Visible).ToListAsync();
                    if (categoryPermissions != null)
                    {
                        visibleSources = new List<string>();
                        foreach (var categoryPermission in categoryPermissions)
                        {
                            foreach (var sourceId in categoryPermission.Sources.Where(s => s.Visible).Select(x => x.SourceId))
                            {
                                visibleSources.Add(sourceId);
                            }
                        }
                    }
                }

                var articles = await Session.Query<Domain.Entities.Articles.Article>().ToListAsync();
                articles = articles.OrderByDescending(x => x.UpdatedDate).Where(x => visibleSources.Contains(x.SourceId)).ToList();
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
                articlesVm = articlesVm.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

                return new Response
                {
                    Articles = articlesVm,
                    SearchTerm = request.SearchTerm,
                    TotalItemCount = total,
                    PageSize = request.PageSize,
                    Page = request.Page
                };
            }
        }
    }
}
