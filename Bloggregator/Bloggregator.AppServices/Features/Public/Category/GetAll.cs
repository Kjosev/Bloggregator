using Bloggregator.AppServices.ViewModels;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Category
{
    public class GetAll
    {
        public class Request : BaseRequest<Response>
        {
            public string SearchTerm { get; set; }
            public string Username { get; set; }
        }

        public class Response : BaseResponse
        {
            public string SearchTerm { get; set; }
            public List<CategoryWithArticlesViewModel> Categories { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var categories = await Session.Query<Domain.Entities.Categories.Category>().Where(x => x.IsActive).ToListAsync();
                var allowedSources = await Session.Query<Domain.Entities.Sources.Source>().Where(x => x.IsActive).ToListAsync();
                var allowdSourcesIds = allowedSources.Select(s => s.Id.ToString()).ToList();

                if (!string.IsNullOrEmpty(request.Username))
                {
                    var categoryPermissions = await Session.Query<Domain.Entities.Categories.CategoryPermission>().Where(x => x.Username == request.Username && x.Visible).ToListAsync();
                    categories = categories.Where(c => categoryPermissions.Select(cp => cp.CategoryId).ToList().Contains(c.Id.ToString())).ToList();
                    var sourceIds = new List<string>();
                    foreach(var cp in categoryPermissions)
                    {
                        sourceIds.AddRange(cp.Sources.Where(s => s.Visible).Select(s => s.SourceId));
                    }
                    allowdSourcesIds = sourceIds;
                }

                Domain.Entities.Account.User user = null;
                if (!string.IsNullOrEmpty(request.Username))
                {
                    user = await Session.Query<Bloggregator.Domain.Entities.Account.User>().Where(x => x.UserName == request.Username).FirstOrDefaultAsync();
                }

                var toReturn = new List<CategoryWithArticlesViewModel>();

                var allArticles = await Session.Query<Domain.Entities.Articles.Article>().ToListAsync();

                foreach (var category in categories)
                {
                   
                    var articles = allArticles.OrderByDescending(x => x.UpdatedDate)
                        .Where(x => allowdSourcesIds.Contains(x.SourceId) && category.SourceIds.Contains(x.SourceId))
                        .Take(4).ToList();

                    var articlesVm = new List<ArticleViewmodel>();

                    foreach (var article in articles)
                    {
                        var articlevm = new ArticleViewmodel(article);
                        articlesVm.Add(articlevm);
                    }

                    if (user != null)
                    {
                        articlesVm.Where(a => user.SavedArticleIds.Contains(a.Id)).ToList().ForEach(a => a.Favorite = true);
                    }

                    toReturn.Add(new CategoryWithArticlesViewModel
                    {
                        Category = category,
                        Articles = articlesVm
                    });
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var toReturnFiltered = new List<CategoryWithArticlesViewModel>();
                    foreach (var model in toReturn)
                    {
                        if (model.Category.Name.ToLower().Contains(request.SearchTerm.ToLower()))
                            toReturnFiltered.Add(model);
                        else
                        {
                            foreach (var article in model.Articles)
                            {
                                if (article.Description.ToLower().Contains(request.SearchTerm.ToLower())
                                    || article.Content.ToLower().Contains(request.SearchTerm.ToLower())
                                    || article.Title.ToLower().Contains(request.SearchTerm.ToLower())
                                    || article.SourceName.ToLower().Contains(request.SearchTerm.ToLower()))
                                {
                                    toReturnFiltered.Add(model);
                                    break;
                                }
                            }
                        }
                    }
                    toReturn = toReturnFiltered;
                }

                return new Response { Categories = toReturn };
            }
        }
    }
}
