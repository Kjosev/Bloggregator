using Bloggregator.AppServices.Features.Admin.Source;
using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bloggregator.Admin.Controllers
{
    [Authorize]
    public class SourcesController : BaseController
    {

        [HttpGet]
        public async Task<ActionResult> Index(List.Request request)
        {
            var model = await Handle(request);

            return View(model);
        }

        public async Task<ActionResult> Edit(Edit.Request request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var response = await Handle(request);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Add.Request request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            await Handle(request);

            return RedirectToAction("Index");
        }
    }
}