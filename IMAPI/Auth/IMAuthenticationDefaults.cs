namespace IM_API.Auth
{
    public class IMAuthenticationDefaults
    {
        public const string AuthenticationScheme = "IM";
        public const string IToken = $"{AuthenticationScheme} (?<token>.*)";
    }
}
