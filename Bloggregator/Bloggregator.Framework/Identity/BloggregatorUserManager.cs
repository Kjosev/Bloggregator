using Bloggregator.Domain.Entities.Account;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bloggregator.Framework.Identity
{
    public class BloggregatorUserManager : UserManager<User>
    {
        public class DisabledUserException : InvalidCredentialException
        {
            public DisabledUserException() : base()
            {
            }

            public DisabledUserException(string message) : base(message)
            {

            }
        }

        public BloggregatorUserManager(IUserStore<User> store) : base(store)
        {
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            user.IsActive = true;
            return await base.CreateAsync(user, password);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            user.IsActive = true;
            return await base.CreateAsync(user);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            if (!user.IsActive)
            {
                throw new DisabledUserException("Cannot create identity for disabled user.");
            }
            return await base.CreateIdentityAsync(user, authenticationType);
        }
    }
}
