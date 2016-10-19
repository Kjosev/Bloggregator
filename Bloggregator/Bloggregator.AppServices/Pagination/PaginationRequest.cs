using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Pagination
{
    public class PaginationRequest<T> : BaseRequest<T> where T : PaginationResponse
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public PaginationRequest()
        {
            Page = 1;
            PageSize = 9;
        }
    }
}
