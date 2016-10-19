using Bloggregator.AppServices;
using MediatR;
using StructureMap.Attributes;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bloggregator.Framework.Web
{
    public class BaseController : Controller
    {
        [SetterProperty]
        public IMediator Mediator { get; set; }

        protected async Task<TResponse> Handle<TResponse>(BaseRequest<TResponse> request)
            where TResponse : BaseResponse
        {
            return await Mediator.SendAsync(request);
        }

    }
}
