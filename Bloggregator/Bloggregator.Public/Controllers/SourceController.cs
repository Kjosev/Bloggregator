using Bloggregator.AppServices.Features.Public.Source;
using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bloggregator.Public.Controllers
{
    public class SourceController : BaseController
    {
        [HttpGet]
        // GET: Source
        public async Task<ActionResult> Index(GetWithArticles.Request request)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                    request.Username = User.Identity.Name;
                var response = await Handle(request);
                if (response.Source != null)
                {
                    return View(response);
                }
            }
            TempData["ErrorMessage"] = "Source not found! Please try again.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Index(GetWithArticles.Request request, string SearchTerm)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                    request.Username = User.Identity.Name;
                var response = await Handle(request);
                if (response.Source != null)
                {
                    ViewBag.Title = "\"" + request.SearchTerm + "\" - Search | " + response.Source.Source.Name + " BlogBuddy";
                    return View(response);
                }
            }
            TempData["ErrorMessage"] = "Source not found! Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }
}