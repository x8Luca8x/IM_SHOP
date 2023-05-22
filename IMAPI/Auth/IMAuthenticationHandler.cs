using IMAPI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace IM_API.Auth
{
    public class IMAuthenticationHandler : AuthenticationHandler<IMAuthenticationOptions>
    {
        private readonly IIMAuthenticationService _authenticationService;

        public IMAuthenticationHandler(IOptionsMonitor<IMAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IIMAuthenticationService authService) 
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
                    return AuthenticateResult.Fail("Missing Authorization Header");

                if (!AuthenticationHeaderValue.TryParse(Request.Headers[HeaderNames.Authorization], out var headerValue))
                    return AuthenticateResult.Fail("Invalid Authorization Header");

                if (!IMAuthenticationDefaults.AuthenticationScheme.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                    return AuthenticateResult.Fail("Invalid Authorization Header");

                var token = Request.Headers[HeaderNames.Authorization].ToString();

                // Get remote machine informations
                var device = new TDEVICE(Request.Headers["Device"].ToString());

                TAUTH auth = await _authenticationService.IsValidUserAsync(token, device);
                if (!auth.IsOK())
                    return AuthenticateResult.Fail(auth.result.ToString());

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, auth.user!.ID.ToString()),
                    new Claim(ClaimTypes.Name, auth.user!.USERNAME),
                    new Claim(ClaimTypes.Email, auth.user!.EMAIL),
                    new Claim(ClaimTypes.Role, auth.user!.ROLE),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(auth.user)),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("INTERNAL_ERROR");
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"IM realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
