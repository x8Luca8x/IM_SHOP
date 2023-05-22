using IMAPI;

namespace IM_API.Auth
{
    public interface IIMAuthenticationService
    {
        Task<TAUTH> IsValidUserAsync(string Token, TDEVICE Device);
    }
}
