using IMAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IM_API.Controllers
{
    [Route("api/Shop/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMDbContext _DbContext;

        public CartController(IMDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            int currentUserId = int.Parse(User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier).Value);
            var cartQuery = from c in _DbContext.Cart
                        where c.USERID == currentUserId
                        select c;

            var cart = await cartQuery.FirstOrDefaultAsync();
            if(cart is null)
                return BadRequest(TRESPONSE.ERROR(LangManager.GetTranslationFromRequest("CART_NOT_FOUND", Request)));

            var cartArticlesQuery = from ca in _DbContext.CartArticle
                                    join a in _DbContext.Article on ca.ARTICLEID equals a.ID
                                    join c in _DbContext.Currency on a.CURRENCYID equals c.ID
                                    where ca.CARTID == cart.ID
                                    select new { CARTARTICLE = ca, ARTICLE = a, CURRENCY = c };

            var cartArticles = await cartArticlesQuery.ToListAsync();
            return Ok(TRESPONSE.OK(new { CART = cart, ARTICLES = cartArticles}));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCart([FromBody] TCARTARTICLE cartArticle)
        {
            if(cartArticle.QUANTITY <= 0)
                return BadRequest(TRESPONSE.ERROR(LangManager.GetTranslationFromRequest("INVALID_ARTICLE_QUANTITY", Request)));

            int currentUserId = int.Parse(User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier).Value);
            var cart = await _DbContext.Cart.FirstOrDefaultAsync(c => c.USERID == currentUserId);

            if (cart == null)
            {
                cart = new TCART
                {
                    USERID = currentUserId
                };

                ModelManager.FillDefaults(cart, true);

                await _DbContext.Cart.AddAsync(cart);
                await _DbContext.SaveChangesAsync();
            }

            var existingCartArticle = await _DbContext.CartArticle.FirstOrDefaultAsync(ca => ca.CARTID == cart.ID && ca.ARTICLEID == cartArticle.ARTICLEID);
            if (existingCartArticle == null)
            {
                cartArticle.CARTID = cart.ID;
                ModelManager.FillDefaults(cartArticle, true);

                await _DbContext.CartArticle.AddAsync(cartArticle);
            }
            else
            {
                ModelManager.FillDefaults(cartArticle);
                existingCartArticle.QUANTITY = cartArticle.QUANTITY;

                _DbContext.CartArticle.Update(existingCartArticle);
            }

            await _DbContext.SaveChangesAsync();
            return Ok(TRESPONSE.OK());
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCartArticle(int ArticleId)
        {
            int currentUserId = int.Parse(User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier).Value);
            var cart = await _DbContext.Cart.FirstOrDefaultAsync(c => c.USERID == currentUserId);

            if (cart == null)
                return BadRequest(TRESPONSE.ERROR(LangManager.GetTranslationFromRequest("CART_NOT_FOUND", Request)));

            var cartArticle = await _DbContext.CartArticle.FirstOrDefaultAsync(ca => ca.CARTID == cart.ID && ca.ARTICLEID == ArticleId);
            if (cartArticle == null)
                return BadRequest(TRESPONSE.ERROR(LangManager.GetTranslationFromRequest("CART_ARTICLE_NOT_FOUND", Request)));

            _DbContext.CartArticle.Remove(cartArticle);
            await _DbContext.SaveChangesAsync();

            return Ok(TRESPONSE.OK());
        }
    }
}
