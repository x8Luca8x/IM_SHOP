using IM_API.Auth;
using IM_API.Security;
using IMAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMDbContext _DbContext;

        public UserController(IMDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TLOGIN Model)
        {
            // Get device information
            string device = Request.Headers["Device"];
            TDEVICE d = new TDEVICE(device);

            string result = await Authentication.Login(_DbContext, Model, d);
            var tokenMatch = Regex.Match(result, IMAuthenticationDefaults.IToken);

            if (tokenMatch.Success)
                return Ok(new { token = tokenMatch.Groups["token"].Value });
            else
            {
                if (result == "INVALID_LOGIN_DATA")
                    return NotFound(result);
                else
                    return BadRequest(result);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] TREGISTER Model)
        {
            string result = await Authentication.Register(_DbContext, Model);
            if (result == "OK")
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUser([FromBody] TAUTHENTICATION Model)
        {
            TAUTH auth = await Authentication.Authenticate(_DbContext, Model.TOKEN, Model.DEVICE);
            if (auth.IsOK())
                return Ok(auth.user);
            else
                return BadRequest(auth);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userString = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.UserData);
            if (userString is null)
                return BadRequest("INVALID_USER");

            return Ok(JsonConvert.DeserializeObject<TUSER_V>(userString.Value));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] TUSERUPDATE Model)
        {
            try
            {
                int currentUserId = int.Parse(User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier).Value);
                TUSER? user = await _DbContext.User.FirstOrDefaultAsync(e => e.ID == currentUserId);

                if (user is null)
                    return BadRequest("INVALID_USER");

                if (Model.USERNAME != null)
                    user.USERNAME = Model.USERNAME;
                if (Model.EMAIL != null)
                    user.EMAIL = Model.EMAIL;
                if (Model.FIRSTNAME != null)
                    user.FIRSTNAME = Model.FIRSTNAME;
                if (Model.LASTNAME != null)
                    user.LASTNAME = Model.LASTNAME;
                if (Model.BIRTHDATE != null)
                    user.BIRTHDATE = Model.BIRTHDATE.Value;

                ModelManager.FillDefaults(user);
                await _DbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("UNKOWN_ERROR");
            }
        }
    }
}