using System;
using System.Collections.Generic;
using System.Linq;
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

    public class DdsAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public DdsAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("DdsClaim", "DdsPatient"));

            var identity = new System.Security.Claims.ClaimsIdentity(claims, "DdsJwt");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
