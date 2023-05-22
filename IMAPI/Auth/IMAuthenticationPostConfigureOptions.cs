using Microsoft.Extensions.Options;

namespace IM_API.Auth
{
    public class IMAuthenticationPostConfigureOptions : IPostConfigureOptions<IMAuthenticationOptions>
    {
        public void PostConfigure(string name, IMAuthenticationOptions options)
        {
            if (string.IsNullOrEmpty(options.Realm))
            {
                throw new InvalidOperationException("Realm must be provided in options");
            }
        }
    }
}
