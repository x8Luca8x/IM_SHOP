using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IM_API.Auth
{
    public static class IMAuthenticationExtensions
    {
        public static AuthenticationBuilder AddIM<TAuthService>(this AuthenticationBuilder builder)
        where TAuthService : class, IIMAuthenticationService
        {
            return AddIM<TAuthService>(builder, IMAuthenticationDefaults.AuthenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddIM<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme)
            where TAuthService : class, IIMAuthenticationService
        {
            return AddIM<TAuthService>(builder, authenticationScheme, _ => { });
        }

        public static AuthenticationBuilder AddIM<TAuthService>(this AuthenticationBuilder builder, Action<IMAuthenticationOptions> configureOptions)
            where TAuthService : class, IIMAuthenticationService
        {
            return AddIM<TAuthService>(builder, IMAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddIM<TAuthService>(this AuthenticationBuilder builder, string authenticationScheme, Action<IMAuthenticationOptions> configureOptions)
            where TAuthService : class, IIMAuthenticationService
        {
            builder.Services.AddSingleton<IPostConfigureOptions<IMAuthenticationOptions>, IMAuthenticationPostConfigureOptions>();
            builder.Services.AddTransient<IIMAuthenticationService, TAuthService>();

            return builder.AddScheme<IMAuthenticationOptions, IMAuthenticationHandler>(
                authenticationScheme, configureOptions);
        }
    }
}
