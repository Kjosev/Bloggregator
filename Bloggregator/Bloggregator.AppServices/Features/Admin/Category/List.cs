using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Bloggregator.AppServices.Pagination;
using Bloggregator.Domain.Entities.Sources;
using Raven.Client;
using Raven.Client.Linq;
using StructureMap.Attributes;

namespace Bloggregator.AppServices.Features.Admin.Category
{
    public class List
    {
        public class Request : PaginationRequest<Response>
        {

        }

        public class Response : PaginationResponse
        {
            public List<Category> Categories { get; set; }
            public class Category
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public bool IsActive { get; set; }
            }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                //Used to get total results
                var stats = new RavenQueryStatistics();

                var categories = await Session.Query<Domain.Entities.Categories.Category>()
                    .Statistics(out stats)
                    .OrderByDescending(x => x.DateCreated)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize).ToListAsync();

                return new Response
                {
                    Categories = categories.Select(x => new Response.Category
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsActive = x.IsActive
                    }).ToList(),
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItemCount = stats.TotalResults
                };
            }
        }
    }
}
