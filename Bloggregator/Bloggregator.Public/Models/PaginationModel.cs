using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggregator.Public.Models
{
    public class PaginationModel
    {
        public string Id { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}