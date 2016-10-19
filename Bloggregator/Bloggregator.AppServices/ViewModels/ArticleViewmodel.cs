using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.ViewModels
{
    public class ArticleViewmodel
    {
        public string Id { get; set; }
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
        public string ImageUrl { get; set; }
        public bool Favorite { get; set; }

        public ArticleViewmodel(Bloggregator.Domain.Entities.Articles.Article article)
        {
            Id = article.Id.ToString();
            Url = article.Url;
            UrlThroughFeed = article.UrlThroughFeed;
            Title = article.Title;
            Description = article.Description;
            Content = article.Content;
            Author = article.Author;
            PublishedDate = article.PublishedDate;
            UpdatedDate = article.UpdatedDate;
            SourceId = article.SourceId;
            SourceName = article.SourceName;
            ImageUrl = article.ImageUrl;
            Favorite = false;
        }
    }
}
