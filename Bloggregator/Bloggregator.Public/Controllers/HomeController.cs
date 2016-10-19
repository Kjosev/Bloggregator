using Bloggregator.AppServices.Features;
using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bloggregator.Public.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(Bloggregator.AppServices.Features.Public.Category.GetAll.Request request)
        {
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;
            var response = await Handle(request);

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string SearchTerm)
        {
            var request = new Bloggregator.AppServices.Features.Public.Category.GetAll.Request();
            request.SearchTerm = SearchTerm;
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;

            var response = await Handle(request);
            ViewBag.Title = "\"" + SearchTerm + "\" - Search | BlogBuddy";
            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> Search(string SearchTerm)
        {
            var request = new Bloggregator.AppServices.Features.Public.Article.GetWithSearch.Request();
            request.SearchTerm = SearchTerm;
            if (User.Identity.IsAuthenticated)
                request.Username = User.Identity.Name;

            var response = await Handle(request);
            ViewBag.Title = "\"" + SearchTerm + "\" - Search | BlogBuddy";
            return View(response);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}