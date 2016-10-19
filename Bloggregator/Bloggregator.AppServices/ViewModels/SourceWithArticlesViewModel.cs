using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.ViewModels
{
    public class SourceWithArticlesViewModel
    {
        public Domain.Entities.Sources.Source Source { get; set; }
        public IList<ArticleViewmodel> Articles { get; set; }
    }
}
