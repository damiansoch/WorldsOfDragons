using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Dragons.WebApi.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return AuthenticateResult.NoResult();
            }
            //if no authorisation heather return fail result
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var passesAuthentication = false;
            string? username = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                //Format:username:password: Convert to Base64 string

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentialString = Encoding.UTF8.GetString(credentialBytes);
                var credentials = credentialString.Split(new[] { ':' }, 2);
                username = credentials[0];
                var password = credentials[1];
                if ("damiansoch@hotmail.com".Equals(username, StringComparison.InvariantCultureIgnoreCase)
                    && "damian1".Equals(password, StringComparison.InvariantCulture))
                {
                    passesAuthentication = true;
                }
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
            //if authentication fails
            if (!passesAuthentication)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
            //if authentication is succeeded
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,username),
                new Claim(ClaimTypes.Name,username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
