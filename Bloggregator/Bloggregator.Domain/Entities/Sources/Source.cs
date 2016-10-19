using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Sources
{
    public class Source : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid CategoryId { get; set; }
    }
}
