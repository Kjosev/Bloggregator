﻿using System;

namespace Bloggregator.Domain.Entities.Account
{
    /// <summary>
    /// Represents the user's e-mail address. This is stored in a seperate document as
    /// we need to ensure the uniqueness of the e-mail address.
    /// </summary>
    /// <remarks>
    /// Storing the e-mail in a seperate document to ensure it's uniqueness by the Id of the document
    /// will only work out if the provided IAsyncDocumentSession is configured for optimistic concurrency.
    /// </remarks>
    public class RavenUserEmail
    {
        public RavenUserEmail(string email, string userId)
        {
            if (email == null) throw new ArgumentNullException("email");
            if (userId == null) throw new ArgumentNullException("userId");

            Id = GenerateKey(email);
            UserId = userId;
            Email = email;
        }

        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Email { get; private set; }

        public ConfirmationRecord ConfirmationRecord { get; private set; }

        public void SetConfirmed()
        {
            if (ConfirmationRecord == null)
            {
                ConfirmationRecord = new ConfirmationRecord();
            }
        }

        public void SetUnconfirmed()
        {
            ConfirmationRecord = null;
        }

        public static string GenerateKey(string email)
        {
            return string.Format(Constants.RavenUserEmailKeyTemplate, email);
        }
    }
}