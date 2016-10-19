using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggregator.AppServices.Pagination;
using Raven.Client;

namespace Bloggregator.AppServices.Features.Admin.Source
{
    public class List
    {
        public class Request : PaginationRequest<Response>
        {
        }

        public class Response : PaginationResponse
        {
            public List<Source> Sources { get; set; }

            public class Source
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public string Url { get; set; }
                public Guid CategoryId { get; set; }
                public bool IsActive { get; set; }
                public string CategoryName { get; set; }
                public bool CategoryIsActive { get; set; }
            }
        }


        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                //Used to get total results
                var stats = new RavenQueryStatistics();

                var sources = await Session.Query<Domain.Entities.Sources.Source>()
                    .Statistics(out stats)
                    .OrderByDescending(x => x.DateCreated)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync();


                var response = new Response
                {
                    Sources = sources.Select(x => new Response.Source
                    {
                        Id = x.Id,
                        Url = x.Url,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        IsActive = x.IsActive,
                    }).ToList(),
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItemCount = stats.TotalResults
                };

                foreach (var source in response.Sources)
                {
                    var category = await Session.LoadAsync<Domain.Entities.Categories.Category>(source.CategoryId);
                    source.CategoryName = category.Name;
                    source.CategoryIsActive = category.IsActive;
                }

                return response;
            }
        }
    }
}
