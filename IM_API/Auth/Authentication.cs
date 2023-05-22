using IM_API.Security;
using IMAPI;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace IM_API.Auth
{
    public static class Authentication
    {
        public static async Task<TAUTH> Authenticate(IMDbContext DbContext, string Token, TDEVICE Device)
        {
            var tokenMatch = Regex.Match(Token, IMAuthenticationDefaults.IToken);
            if (!tokenMatch.Success)
                return new TAUTH(null, EAuthResult.INVALID_TOKEN);

            var tokenString = tokenMatch.Groups["token"].Value;
            var query = from t in DbContext.Token
                        join u in DbContext.User on t.USERID equals u.ID
                        where t.TOKEN == tokenString && t.DEVICE_NAME == Device.name && t.DEVICE_OS == Device.os && t.DEVICE_APP == Device.app
                        select new { TOKEN = t, USER = u as TUSER_V };

            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return new TAUTH(null, EAuthResult.INVALID_TOKEN);
            else if (!result.USER.VERIFIED)
                return new TAUTH(null, EAuthResult.NOT_VERIFIED_USER);
            else if (!result.USER.ACTIVE)
                return new TAUTH(null, EAuthResult.INACTIVE_USER);
            else if (result.TOKEN.CAN_EXPIRE && result.TOKEN.EXPIRES < DateTime.Now)
                return new TAUTH(null, EAuthResult.TOKEN_EXPIRED);

            return new TAUTH(result.USER, EAuthResult.OK);
        }

        public static async Task<string> Login(IMDbContext DbContext, TLOGIN Model, TDEVICE Device)
        {
            string token = Guid.NewGuid().ToString();
            var query = from u in DbContext.User
                        where u.USERNAME == Model.USERNAME || u.EMAIL == Model.USERNAME
                        select u;

            var users = query.ToList();
            TUSER? user = null;

            foreach (var row in users)
            {
                if(IMSecurity.ComparePassword(Model.PASSWORD, row.PASSWORD))
                {
                    user = row;
                    break;
                }
            }

            if (user is null)
                return "INVALID_LOGIN_DATA";

            if(!user.ACTIVE)
                return "INACTIVE_USER";
            if(!user.VERIFIED)
                return "NOT_VERIFIED_USER";

            var tokenObj = new TTOKEN()
            {
                USERID = user.ID,
                TOKEN = token,
                DEVICE_NAME = Device.name,
                DEVICE_OS = Device.os,
                DEVICE_APP = Device.app,
                CAN_EXPIRE = Model.CAN_EXPIRE,
                EXPIRES = Model.EXPIRES
            };

            ModelManager.FillDefaults(tokenObj, true);
            DbContext.Token.Add(tokenObj);

            try
            {
                await DbContext.SaveChangesAsync();
                return "IM " + token;
            }
            catch (Exception)
            {
                return "UNKNOWN_ERROR";
            }
        }

        public static async Task<string> Register(IMDbContext DbContext, TREGISTER Model)
        {
            if (Model.PASSWORD != Model.PASSWORD_CONFIRM)
                return "PASSWORDS_DO_NOT_MATCH";

            var query = from u in DbContext.User
                        where u.USERNAME == Model.USERNAME || u.EMAIL == Model.EMAIL
                        select u;

            var users = query.ToList();
            foreach (var row in users)
            {
                if (row.USERNAME == Model.USERNAME)
                    return "USERNAME_ALREADY_EXISTS";
                else if (row.EMAIL == Model.EMAIL)
                    return "EMAIL_ALREADY_EXISTS";
            }

            var user = new TUSER()
            {
                USERNAME = Model.USERNAME,
                EMAIL = Model.EMAIL,
                PASSWORD = IMSecurity.IMHash(Model.PASSWORD),
                FIRSTNAME = Model.FIRSTNAME,
                LASTNAME = Model.LASTNAME,
                BIRTHDATE = Model.BIRTHDATE,
                VERIFIED = false,
                ACTIVE = true
            };

            ModelManager.FillDefaults(user, true);
            DbContext.User.Add(user);

            try
            {
                await DbContext.SaveChangesAsync();

                // Add new TUSEROPTIONS and TPERSON
                var userOptions = new TUSEROPTIONS()
                {
                    USERID = user.ID
                };

                var person = new TPERSON()
                {
                    USERID = user.ID,
                    DISPLAYNAME = user.USERNAME,
                };

                ModelManager.FillDefaults(userOptions, true);
                ModelManager.FillDefaults(person, true);

                DbContext.UserOptions.Add(userOptions);
                DbContext.Person.Add(person);

                await DbContext.SaveChangesAsync();
                return "OK";
            }
            catch (Exception)
            {
                return "UNKNOWN_ERROR";
            }
        }
    }
}
