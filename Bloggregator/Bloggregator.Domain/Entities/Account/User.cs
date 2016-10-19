using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Domain.Entities.Account
{
    public class User : RavenUser
    {
        public User(string userName)
            : base(userName)
        {
            SavedArticleIds = new List<string>();
            SourceIds = new List<string>();
        }

        public List<string> SourceIds { get; set; }
        public List<string> SavedArticleIds { get; set; }
    }
}
