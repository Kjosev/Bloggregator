using Bloggregator.Domain.Entities.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Categories
{
    public class CategoryPermission : BaseEntity
    {
        public string Username { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<SourcePermission> Sources { get; set; }
        public bool Visible { get; set; }
    }
}
