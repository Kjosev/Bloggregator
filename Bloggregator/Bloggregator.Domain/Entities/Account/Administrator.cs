using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Account
{
    public class Administrator : RavenUser
    {
        public Administrator(string userName)
            : base(userName)
        {
        }
    }
}
