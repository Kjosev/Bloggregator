using Bloggregator.AppServices.Features.Admin;
using Bloggregator.AppServices.Features.Admin.User;
using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Bloggregator.Admin.Controllers
{
    public class UsersController : BaseController
    {
        public async Task<ActionResult> Index(List.Request Request)
        {
            return View(await Handle(Request));
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
    }
}