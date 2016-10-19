using Bloggregator.AppServices.Features;
using Bloggregator.AppServices.Features.Articles;
using Bloggregator.Domain.Entities.Articles;
using Bloggregator.Domain.Entities.Categories;
using Bloggregator.Domain.Entities.Sources;
using Bloggregator.Framework;
using MediatR;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Bloggregator.Crawler
{
    class Program
    {
        public const int MINUTES = 15;
        public const int SECONDS_IN_MINUTE = 60;
        public const int MILISECONDS_IN_SECOND = 1000;
        static void Main(string[] args)
        {
            Bootstrapper bootstrapper = new Bootstrapper(new BootstrapperSettings());
            var documentStore = ServiceLocator.Current.GetInstance<IDocumentStore>();

            while (true)
            {
                Task.Run(async () =>
                {
                try
                {
                    IList<Source> sources = new List<Source>();
                        using (var session = documentStore.OpenAsyncSession())
                        {
                            sources = await session.Query<Source>().ToListAsync();
                        }

                        var articlesList = new List<Article>();

                        foreach (var source in sources)
                        {
                            var xmlReader = XmlReader.Create(source.Url);
                            var feed = SyndicationFeed.Load(xmlReader);
                            xmlReader.Close();

                            // there are only 10 items per source
                            // checking if item exists will be called only 10 times
                            // so it's okay for these calls to the db to share the same session
                            foreach (var item in feed.Items)
                            {
                                var feedId = item.Id;

                                using (var session = documentStore.OpenAsyncSession())
                                {
                                    var exists = await session.Query<Article>().AnyAsync(a => a.UrlThroughFeed == item.Id);

                                    if(exists)
                                    {
                                        Console.WriteLine("Article with feed id '{0}' is already saved", feedId);
                                        continue;
                                    }
                                }
                                
                                var article = new Article
                                {
                                    DateCreated = DateTime.Now,
                                    IsActive = true,
                                    Url = item.Links.Count > 0 ? item.Links[0].Uri.AbsoluteUri : item.Id,
                                    UrlThroughFeed = item.Id,
                                    Title = item.Title.Text,
                                    Description = item.Summary?.Text,
                                    PublishedDate = item.PublishDate.DateTime,
                                    UpdatedDate = item.LastUpdatedTime.DateTime < item.PublishDate.DateTime ? item.PublishDate.DateTime : item.LastUpdatedTime.DateTime,
                                    SourceId = source.Id.ToString(),
                                    SourceName = source.Name,
                                    SourceUrl = source.Url
                                };

                                // author is defined in a tag not recognizable by syndicationfeed
                                var authorExtension = item.ElementExtensions.Where(e => e.OuterName.Contains("creator")).FirstOrDefault();
                                if (authorExtension != null)
                                {
                                    var xmlNode = XElement.ReadFrom(authorExtension.GetReader());
                                    article.Author = (xmlNode as XElement).Value;
                                }
                                else
                                {
                                    article.Author = "Unknown";
                                }

                                // content is not present at all feeds
                                if (item.Content != null)
                                {
                                    if (item.Content is TextSyndicationContent)
                                        article.Content = (item.Content as TextSyndicationContent).Text;
                                }
                                else
                                {
                                    var contentExtension = item.ElementExtensions.Where(e => e.OuterName.Contains("encoded") || e.OuterNamespace.Contains("content")).FirstOrDefault();
                                    if (contentExtension != null)
                                    {
                                        var xmlNode = XElement.ReadFrom(contentExtension.GetReader());
                                        article.Content = (xmlNode as XElement).Value;
                                    }
                                    else
                                    {
                                        article.Content = "Not available";
                                    }
                                }

                                //description is not present at all feeds 
                                if (item.Summary == null)
                                {
                                    var descriptionExtension = item.ElementExtensions.Where(e => e.OuterName.Contains("encoded") || e.OuterNamespace.Contains("description")).FirstOrDefault();
                                    if (descriptionExtension != null)
                                    {
                                        var xmlNode = XElement.ReadFrom(descriptionExtension.GetReader());
                                        article.Description = (xmlNode as XElement).Value;
                                    }
                                    else
                                    {
                                        article.Description = "Not available";
                                    }
                                }

                                var imgFoundInLinks = false;
                                foreach(var link in item.Links)
                                {
                                    if (link.MediaType == "image/jpeg")
                                    {
                                        imgFoundInLinks = true;
                                        article.ImageUrl = link.Uri.AbsoluteUri;
                                    }
                                }

                                if (!imgFoundInLinks)
                                {
                                    if (article.Description.Contains("<img "))
                                    {
                                        var startindex = article.Description.IndexOf("<img ");
                                        var remaining = article.Description.Substring(startindex);
                                        var srcindex = remaining.IndexOf("src=") + 5;
                                        var srcremaining = remaining.Substring(srcindex);
                                        var endquote = srcremaining.IndexOf("\"");
                                        var img = srcremaining.Substring(0, endquote);
                                        article.ImageUrl = img;
                                    }
                                    else if (article.Content.Contains("<img "))
                                    {
                                        var startindex = article.Content.IndexOf("<img ");
                                        var remaining = article.Content.Substring(startindex);
                                        var srcindex = remaining.IndexOf("src=") + 5;
                                        var srcremaining = remaining.Substring(srcindex);
                                        var endquote = srcremaining.IndexOf("\"");
                                        var img = srcremaining.Substring(0, endquote);
                                        article.ImageUrl = img;
                                    }
                                    else
                                    {
                                        if (feed.ImageUrl != null)
                                        {
                                            article.ImageUrl = feed.ImageUrl.AbsoluteUri;
                                        }
                                    }
                                }

                                articlesList.Add(article);
                               
                            }
                        }
                        if (articlesList.Count > 0)
                        {
                            using (var session = documentStore.OpenAsyncSession())
                            {
                                foreach (var article in articlesList)
                                {
                                    await session.StoreAsync(article);
                                    Console.WriteLine("Stored '{0}' to database", article.Title);
                                }
                                await session.SaveChangesAsync();
                                Console.WriteLine("Saved changes");
                                articlesList.Clear();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        var m = e.Message;
                        Console.WriteLine(m);
                    }
                });
                Console.WriteLine("==========================================");
                Console.WriteLine("Crawling task is running asynchronously");
                Console.WriteLine("Thread going to sleep for {0} minutes", MINUTES);
                Console.WriteLine("==========================================");
                Thread.Sleep(MINUTES * SECONDS_IN_MINUTE * MILISECONDS_IN_SECOND);
            }
        }
    }
}
