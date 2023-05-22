using IMAPI;

namespace IM_API.Auth
{
    public class IMAuthenticationService : IIMAuthenticationService
    {
        public async Task<TAUTH> IsValidUserAsync(string Token, TDEVICE Device)
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<IMDbContext>();
            optionsBuilder.UseMySql(IMDbContext.ConnectionString, ServerVersion.AutoDetect(IMDbContext.ConnectionString));

            using (var db = new IMDbContext(optionsBuilder.Options))
            {
                return await Authentication.Authenticate(db, Token, Device);
            }
        }
    }
}
