using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.ViewModels
{
    public class CategoryWithArticlesViewModel
    {
        public Domain.Entities.Categories.Category Category { get; set; }
        public IList<ArticleViewmodel> Articles { get; set; }
    }
}
