using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Bloggregator.Public.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Twitter;

namespace Bloggregator.Public
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");


            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = "Ymu8lDNUtjN5ZyWrQBlCzP3zF",
                ConsumerSecret = "my8CZQeKsfb2HSQmOYbR7IISosgHkbKigkly634UckYRBg6a4L",
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                    "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                    "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                    "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                    "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                    "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
                    "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                })
            });

            app.UseFacebookAuthentication(
               appId: "1704667546465742",
               appSecret: "4f856464c3b62d1eff4aa158ec44a654");

            app.UseGoogleAuthentication(
                clientId: "57425486396-b7iqvrug99r0rurgru3h4d6dijlb7m07.apps.googleusercontent.com",
                clientSecret: "-COTMbyKs5AfasDq1AcSsHMS");
        }
    }
}