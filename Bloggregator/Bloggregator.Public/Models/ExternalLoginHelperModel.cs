using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bloggregator.Public.Models
{
    public class ExternalLoginHelperModel
    {
        public string FacebookProviderText { get; set; }
        public string TwitterProviderText { get; set; }
        public string GoogleProviderText { get; set; }
    }
}