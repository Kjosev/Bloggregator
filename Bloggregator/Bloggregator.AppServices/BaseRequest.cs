using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.AppServices
{
    public abstract class BaseRequest<TResponse> : IAsyncRequest<TResponse>
       where TResponse : BaseResponse
    {
    }
}
