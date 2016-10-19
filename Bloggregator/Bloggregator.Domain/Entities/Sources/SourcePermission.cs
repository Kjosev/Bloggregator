using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Sources
{

    public class SourcePermission : BaseEntity
    {
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
        public bool Visible { get; set; }
    }
}
