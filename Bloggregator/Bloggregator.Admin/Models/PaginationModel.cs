using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggregator.Admin.Models
{
    public class PaginationModel
    {
        public int PageCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}