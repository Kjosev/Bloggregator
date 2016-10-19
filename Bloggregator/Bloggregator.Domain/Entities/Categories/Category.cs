using Bloggregator.Domain.Entities.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Categories
{
    public class Category : BaseEntity
    {
        public Category()
        {
            SourceIds = new List<string>();
        }

        public string Name { get; set; }
        public List<string> SourceIds { get; set; }
    }
}
