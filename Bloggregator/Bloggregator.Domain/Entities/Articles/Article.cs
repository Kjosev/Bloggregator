using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Articles
{
    public class Article : BaseEntity
    {
        public string Url { get; set; }
        public string UrlThroughFeed { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
