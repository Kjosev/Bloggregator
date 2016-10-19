using Bloggregator.Domain.Entities.Categories;
using Bloggregator.Domain.Entities.Sources;
using Bloggregator.Domain.Entities.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices.Features
{
    public class ReadSourcesFeature
    {
        public class Request : BaseRequest<Response>
        {
        }

        public class Response : BaseResponse
        {
            public IList<Source> Sources { get; set; }
        }

        public class Handler : BaseHandler<Request, Response>
        {
            public override async Task<Response> Handle(Request request)
            {
                var results = await Session.Advanced.AsyncDocumentQuery<Source>().ToListAsync();

                return new Response
                {
                    Sources = results
                };
            }
        }
    }
}
