using IMAPI;

namespace IM_API.Auth
{
    public class IMDefaultAuthenticationService : IIMAuthenticationService
    {
        public async Task<TAUTH> IsValidUserAsync(string Token, TDEVICE Device)
        {
            return await IMAPI.IMAPI.Authenticate(Token, Device);
        }
    }
}
