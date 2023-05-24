using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IMDbContext _DbContext;

        public ShopController(IMDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        // @param OrderBy: 0 = Newest, 1 = Oldest, 2 = Most Popular
        public async Task<IActionResult> Get(int ArticleId = 0, int PersonId = 0, int OrderBy = 0)
        {
            var query = from a in _DbContext.Article
                        join p in _DbContext.Person on a.PERSONID equals p.ID
                        join c in _DbContext.Currency on a.CURRENCYID equals c.ID
                        where (ArticleId == 0 || a.ID == ArticleId) && (PersonId == 0 || a.PERSONID == PersonId)
                        select new { a, p, c };

            switch (OrderBy)
            {
                case 0:
                    query = query.OrderByDescending(a => a.a.CREATED);
                    break;
                case 1:
                    query = query.OrderBy(a => a.a.CREATED);
                    break;
                case 2:
                    query = query.OrderByDescending(a => a.a.VIEWS);
                    break;
            }

            var result = await query.ToListAsync();
            return Ok(TRESPONSE.OK(result, result.Count == 0 ? "No article found" : string.Empty));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] TARTICLE Model)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string result = await ShopManager.CreateArticle(_DbContext, Model, userId);

            if (result == "OK")
                return Ok(TRESPONSE.OK(Model.ID));
            else
                return BadRequest(TRESPONSE.ERROR(result));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] TARTICLE Model)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string result = await ShopManager.UpdateArticle(_DbContext, Model, userId);

            if (result == "OK")
                return Ok(TRESPONSE.OK());
            else
                return BadRequest(TRESPONSE.ERROR(result));
        }

        [HttpPut("View")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewArticle(int ArticleId)
        {
            int userId = 0;
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if(claim != null)
                userId = int.Parse(claim.Value);

            string result = await ShopManager.ViewArticle(_DbContext, ArticleId, userId);
            if (result == "OK")
                return Ok(TRESPONSE.OK());
            else
                return BadRequest(TRESPONSE.ERROR(result));
        }
    }
}
