using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Framework
{
    public abstract class AbstractBootstrapperSettings
    {
        public string DatabaseName
        {
            get
            {
                var str = ConfigurationManager.AppSettings["DatabaseName"];

                if (string.IsNullOrEmpty(str))
                    throw new NotImplementedException("DatabaseName is not set");

                return str;
            }
        }

        public string DatabaseUrl
        {
            get
            {
                var str = ConfigurationManager.AppSettings["DatabaseUrl"];

                if (string.IsNullOrEmpty(str))
                    throw new NotImplementedException("DatabaseUrl is not set");

                return str;
            }
        }
    }
}
