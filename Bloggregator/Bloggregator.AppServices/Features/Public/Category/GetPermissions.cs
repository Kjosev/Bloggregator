using Bloggregator.Domain.Entities.Categories;
using Bloggregator.Domain.Entities.Sources;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features.Public.Category
{
    public class GetPermissions
    {
        public class Request : BaseRequest<Response>
        {
            public string Username { get; set; }
        }

        public class Response : BaseResponse
        {
            public IList<Bloggregator.Domain.Entities.Categories.CategoryPermission> Categories { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var categories = await Session.Query<Domain.Entities.Categories.CategoryPermission>().Where(x => x.Username == request.Username).ToListAsync();
                var allcategories = await Session.Query<Domain.Entities.Categories.Category>().ToListAsync();

                //bugfix S016C001
                //adding a new source to an existing category -> articles from that source are not shown
                var allsources = await Session.Query<Domain.Entities.Sources.Source>().ToListAsync();
                foreach(var categoryPermission in categories)
                {
                    var cat = allcategories.Where(c => c.Id.ToString() == categoryPermission.CategoryId).FirstOrDefault();
                    if (cat.SourceIds.Count > categoryPermission.Sources.Count) // new source is added to this category
                    {
                        var newSources = cat.SourceIds.Where(si => !categoryPermission.Sources.Select(x => x.SourceId).Contains(si)).ToList();
                        foreach(var newSourceId in newSources)
                        {
                            var source = allsources.Where(s => s.Id.ToString() == newSourceId).FirstOrDefault();
                            var newSourcePermission = new SourcePermission
                            {
                                DateCreated = DateTime.Now,
                                IsActive = true,
                                Visible = true,
                                SourceId = source.Id.ToString(),
                                SourceName = source.Name,
                                SourceUrl = source.Url
                            };

                            var savePermission = await Session.LoadAsync<CategoryPermission>(categoryPermission.Id);
                            savePermission.Sources.Add(newSourcePermission);
                            await Session.SaveChangesAsync();
                        }
                    }
                }



                if (allcategories.Count > categories.Count)
                {
                    var notinsertedcategories = allcategories.Where(x => !categories.Select(c => c.CategoryId).ToList().Contains(x.Id.ToString())).ToList();
                    foreach (var newcategory in notinsertedcategories)
                    {
                        var entity = new Domain.Entities.Categories.CategoryPermission
                        {
                            CategoryId = newcategory.Id.ToString(),
                            CategoryName = newcategory.Name,
                            Username = request.Username,
                            DateCreated = DateTime.Now,
                            Visible = true
                        };
                        var sources = allsources.Where(s => newcategory.SourceIds.Contains(s.Id.ToString())).ToList();
                        var sourcespermissions = new List<Domain.Entities.Sources.SourcePermission>();
                        foreach (var newsource in sources)
                        {
                            var sourcepermission = new Domain.Entities.Sources.SourcePermission
                            {
                                SourceId = newsource.Id.ToString(),
                                SourceName = newsource.Name,
                                SourceUrl = newsource.Url,
                                Visible = true,
                                DateCreated = DateTime.Now
                            };
                            sourcespermissions.Add(sourcepermission);
                        }
                        entity.Sources = sourcespermissions;

                        await Session.StoreAsync(entity);
                        await Session.SaveChangesAsync();

                        categories.Add(entity);
                    }
                }
                // check if all categories still exist and if they are active
                var activeCategories = new List<Domain.Entities.Categories.CategoryPermission>();
                foreach(var category in categories)
                {
                    if (allcategories.Select(x => x.Id.ToString()).Contains(category.CategoryId) && allcategories.Where(x => x.Id.ToString() == category.CategoryId).FirstOrDefault().IsActive)
                    {
                        var cat = allcategories.Where(x => x.Id.ToString() == category.CategoryId).FirstOrDefault();
                        category.Sources = category.Sources.Where(s => cat.SourceIds.Contains(s.SourceId)).ToList();
                        activeCategories.Add(category);
                    }
                }

                return new Response { Categories = activeCategories };
            }
        }
    }
}
