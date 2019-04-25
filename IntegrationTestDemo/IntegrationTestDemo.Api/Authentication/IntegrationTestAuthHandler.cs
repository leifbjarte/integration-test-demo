using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Authentication
{
    internal class IntegrationTestAuthHandler : AuthenticationHandler<IntegrationTestAuthOptions>
    {
        public IntegrationTestAuthHandler(IOptionsMonitor<IntegrationTestAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var claims = GatherClaims();
                var testIdentity = new ClaimsIdentity(claims, "IntegrationTestAuth");
                var testUser = new ClaimsPrincipal(testIdentity);
                var ticket = new AuthenticationTicket(testUser, new AuthenticationProperties(), "IntegrationTestAuth");

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail(ex));
            }
        }

        private List<Claim> GatherClaims()
        {
            var tokenDeserialized = new
            {
                UniqueId = Guid.Empty,
                Name = string.Empty,
                Email = string.Empty
            };

            try
            {
                var token = Request.Headers["Authorization"].ToString();
                var tokenPart = token.Split(' ')[1];
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(tokenPart));
                tokenDeserialized = JsonConvert.DeserializeAnonymousType(decoded, tokenDeserialized);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to extract test auth token from [Authorization] header. See inner exception for details.", ex);
            }

            var claims = new List<Claim>
            {
                new Claim("http://schemas.integrationtest.com/identity/claims/uniqueid", tokenDeserialized.UniqueId.ToString()),
                new Claim("http://schemas.integrationtest.com/identity/claims/name", tokenDeserialized.Name),
                new Claim("http://schemas.integrationtest.com/identity/claims/email", tokenDeserialized.Email)
            };

            //TODO: Add whatever claims used in your API.

            return claims;
        }
    }
}
