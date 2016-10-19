using Bloggregator.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Bloggregator.AppServices.Features.Admin.Category;
using Bloggregator.AppServices.Features.Admin.Source;
using Bloggregator.Domain.Entities.Categories;

namespace Bloggregator.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Users");
        }

        /// <summary>
        /// TEST METHOD!!! Should be removed.
        /// </summary>
        /// <param name="numSamples"></param>
        /// <returns></returns>
        //public async Task<ActionResult> GenerateTestData(int numSamples = 5)
        //{
        //    AppServices.Features.Admin.Source.Add.Request request = new AppServices.Features.Admin.Source.Add.Request();

        //    IList<Category> categories = (await Handle(new List.FindAllCategoriesRequest()))
        //        .GetCategoryEntities().ToList();

        //    var random = new Random((int) DateTime.Now.Ticks);

        //    for (int i = 0; i < numSamples; ++i)
        //    {
        //        int index = random.Next(categories.Count());

        //        request.CategoryId = categories[index].Id.ToString();

        //        request.Name = "Random sample";
        //        request.Url = "Random URL";

        //        await Task.Delay(1000);

        //        await Handle(request);
        //    }

        //    return RedirectToAction("Sources");
        //}
    }
}