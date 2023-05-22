using IM_API.Auth;
using IMAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMDbContext _DbContext;

        public PersonController(IMDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPerson(int PersonId = 0)
        {
            var query = from p in _DbContext.Person
                        where p.ID == PersonId || PersonId == 0
                        join u in _DbContext.User on p.USERID equals u.ID
                        join uo in _DbContext.UserOptions on u.ID equals uo.USERID
                        select new { PERSON = p, USER = u as TUSER_V, USEROPTIONS = uo };

            var result = await query.ToListAsync();
            List<TPERSON_V> resultList = new List<TPERSON_V>();

            foreach (var item in result)
                resultList.Add(PersonManager.MakePersonView(item.PERSON, item.USER, item.USEROPTIONS));

            return Ok(resultList);
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> SearchPerson(string SearchString)
        {
            var query = from p in _DbContext.Person
                        join u in _DbContext.User on p.USERID equals u.ID
                        join uo in _DbContext.UserOptions on u.ID equals uo.USERID
                        where u.USERNAME.ToUpper().Contains(SearchString.ToUpper())
                        || u.EMAIL.ToUpper().Contains(SearchString.ToUpper())
                        || u.FIRSTNAME.ToUpper().Contains(SearchString.ToUpper())
                        || u.LASTNAME.ToUpper().Contains(SearchString.ToUpper())
                        select new { PERSON = p, USER = u as TUSER_V, USEROPTIONS = uo };

            var result = await query.ToListAsync();
            List<TPERSON_V> resultList = new List<TPERSON_V>();

            foreach (var item in result)
                resultList.Add(PersonManager.MakePersonView(item.PERSON, item.USER, item.USEROPTIONS));

            return Ok(resultList);
        }

        [HttpGet("Image")]
        [Authorize]
        public async Task<IActionResult> GetPersonImage(int PersonId)
        {
            var query = from i in _DbContext.Image
                        where i.PERSONID == PersonId
                        select i;

            var result = await query.FirstOrDefaultAsync();
            if(result is null)
                return NotFound();

            return File(result.DATA, "image/" + result.TYPE);
        }

        [HttpGet("Image/Anonym")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnonymImage(int PersonId, string IMToken, TDEVICE Device)
        {
            TAUTH auth = await Authentication.Authenticate(_DbContext, IMToken, Device);
            if(!auth.IsOK())
                return Unauthorized();

            var query = from i in _DbContext.Image
                        where i.PERSONID == PersonId
                        select i;

            var result = await query.FirstOrDefaultAsync();
            if (result is null)
                return NotFound();

            return File(result.DATA, "image/" + result.TYPE);
        }
    }
}
