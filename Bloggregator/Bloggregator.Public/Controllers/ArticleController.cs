using Bloggregator.AppServices.Features.Public.Article;
using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bloggregator.Public.Controllers
{
    [Authorize]
    public class ArticleController : BaseController
    {
        [HttpGet]
        // GET: Article
        public async Task<ActionResult> Index(GetFavourites.Request request)
        {
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;

            var response = await Handle(request);

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> Index(GetFavourites.Request request, string SearchTerm)
        {
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;

            var response = await Handle(request);
            ViewBag.Title = "\"" + request.SearchTerm + "\" - Search | Favourites BlogBuddy";
            return View(response);
        }

        [Route("update")]
        [HttpPost]
        public async Task<ActionResult> UpdateFavoriteArticle(Bloggregator.AppServices.Features.Public.Article.UpdateFavorite.Request request)
        {
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;

            var response = await Handle(request);
            return new HttpStatusCodeResult(response.StatusCode);
        }
    }
}