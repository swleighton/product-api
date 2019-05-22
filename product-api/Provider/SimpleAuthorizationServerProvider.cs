using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using productapi.Data;
using productapi.Helpers;

namespace productapi.Provider
{

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                if (UserData.ValidateUser(context.UserName, context.Password))
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("sub", context.UserName));
                    context.Validated(identity);
                }
                else
                {
                    context.Response.OnSendingHeaders(state =>
                    {
                        var resp = (OwinResponse)state;
                        resp.StatusCode = 401;
                    }, context.Response);
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }catch(Exception e)
            {
                context.SetError("server-error", e.Message);
                context.Response.OnSendingHeaders(state =>
                {
                    var resp = (OwinResponse)state;
                    resp.StatusCode = 500;
                }, context.Response);

                return;
            }
        }
    }
}