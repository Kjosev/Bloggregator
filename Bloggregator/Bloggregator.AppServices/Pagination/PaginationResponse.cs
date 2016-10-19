using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Pagination
{
    public class PaginationResponse : BaseResponse
    {
        public int PageSize { get; set; }

        public int Page { get; set; }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItemCount / PageSize);
            }
        }
        public long TotalItemCount { get; set; }
    }
}
