using MediatR;
using Raven.Client;
using StructureMap.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices
{
    public abstract class BaseHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
       where TRequest : BaseRequest<TResponse>
       where TResponse : BaseResponse
    {
        [SetterProperty]
        public IAsyncDocumentSession Session { get; set; }

        public abstract Task<TResponse> Handle(TRequest request);
    }
}
