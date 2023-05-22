using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly IMDbContext _DbContext;

        public AdminController(IMDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet("User")]
        public async Task<IActionResult> GetUsers(int UserId = 0, bool Options = false)
        {
            if (Options)
            {
                var query = from u in _DbContext.User
                            join uo in _DbContext.UserOptions on u.ID equals uo.USERID
                            where u.ID == UserId || UserId == 0
                            select new { USER = u as TUSER_V, OPTIONS = uo };

                var users = await query.ToListAsync();
                return Ok(users);
            }
            else
            {
                var query = from u in _DbContext.User
                            where u.ID == UserId || UserId == 0
                            select u as TUSER_V;

                var users = await query.ToListAsync();
                return Ok(users);
            }
        }
    }
}
