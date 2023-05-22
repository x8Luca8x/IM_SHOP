using Microsoft.AspNetCore.Authentication;

namespace IM_API.Auth
{
    public class IMAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }
}
