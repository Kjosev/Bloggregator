using Bloggregator.AppServices.Features.Public.Category;
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
    public class ManageController : BaseController
    {
        // GET: Manage
        public async Task<ActionResult> Index(GetPermissions.Request request)
        {
            request.Username = User.Identity.Name;
            var response = await Handle(request);

            return View(response.Categories);
        }

        [HttpPost]
        public async Task<ActionResult> Index(IList<Bloggregator.Domain.Entities.Categories.CategoryPermission> model)
        {
            var request = new UpdatePermissions.Request { Categories = model };
            var response = await Handle(request);

            if (response.Categories != null)
                TempData["Success"] = "Changes saved successfully!";

            return RedirectToAction("Index");
        }
    }
}