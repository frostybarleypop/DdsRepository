using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DDSPatient
{
    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TokenAuthenticationOptions()
        {

        }
    }

    public class DdsAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public DdsAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }
            var creds = authHeader.ToString().Split(":".ToCharArray());
            if (creds.Length !=2)
            {
                return AuthenticateResult.Fail("Malformed Authorization Header");
            }

            Credentials cred = new Credentials { username = creds[0], password = creds[1] };

            if (!cred.username.Equals(cred.password, StringComparison.InvariantCultureIgnoreCase))    
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("DdsClaim", "DdsPatient"));
            claims.Add(new Claim(ClaimTypes.Name, cred.username));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, cred.username));

            var identity = new System.Security.Claims.ClaimsIdentity(claims, "DdsJwt");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
