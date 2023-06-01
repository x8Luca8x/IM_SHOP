using IMAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace IM_API.Controllers
{
    public class ShopSearchResult
    {
        public TARTICLE ARTICLE { get; set; }
        public TPERSON_V PERSON { get; set; }
        public TCURRENCY CURRENCY { get; set; }
    }

    public class ArticleInformation
    {
        public int ARTICLEID { get; set; }
        public int NUMIMAGES { get; set; }
    }

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
        public async Task<IActionResult> Get(int ArticleId = 0, int PersonId = 0, int OrderBy = 0, int Limit = 0)
        {
            var query = from a in _DbContext.Article
                        join p in _DbContext.Person on a.PERSONID equals p.ID
                        join c in _DbContext.Currency on a.CURRENCYID equals c.ID
                        where (ArticleId == 0 || a.ID == ArticleId) && (PersonId == 0 || a.PERSONID == PersonId)
                        select new { ARTICLE = a, PERSON = p, CURRENCY = c };

            if (Limit > 0)
                query = query.Take(Limit);

            switch (OrderBy)
            {
                case 0:
                    query = query.OrderByDescending(a => a.ARTICLE.CREATED);
                    break;
                case 1:
                    query = query.OrderBy(a => a.ARTICLE.CREATED);
                    break;
                case 2:
                    query = query.OrderByDescending(a => a.ARTICLE.VIEWS);
                    break;
            }

            var result = await query.ToListAsync();
            return Ok(TRESPONSE.OK(Request, result, result.Count == 0 ? LangManager.GetTranslationFromRequest("NO_ARTICLE_FOUND", Request) : string.Empty));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] TARTICLE Model)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string result = await ShopManager.CreateArticle(_DbContext, Model, userId);

            if (result == "OK")
                return Ok(TRESPONSE.OK(Request, Model.ID));
            else
                return BadRequest(TRESPONSE.ERROR(Request, result));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] TARTICLE Model)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            string result = await ShopManager.UpdateArticle(_DbContext, Model, userId);

            if (result == "OK")
                return Ok(TRESPONSE.OK(Request));
            else
                return BadRequest(TRESPONSE.ERROR(Request, result));
        }

        [HttpPut("View")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewArticle(int ArticleId)
        {
            int userId = 0;
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
                userId = int.Parse(claim.Value);

            string result = await ShopManager.ViewArticle(_DbContext, ArticleId, userId);
            if (result == "OK")
                return Ok(TRESPONSE.OK(Request));
            else
                return BadRequest(TRESPONSE.ERROR(Request, result));
        }

        [HttpGet("Search")]
        [AllowAnonymous]
        // @param OrderBy: 0 = Newest, 1 = Oldest, 2 = Most Popular, 3 = Price Low to High, 4 = Price High to Low, 5 = Quantity Low to High, 6 = Quantity High to Low, 7 = Title A to Z, 8 = Title Z to A, 9 = Views Low to High, 10 = Views High to Low
        public async Task<IActionResult> Search(string SearchText, int OrderBy = 0, int Page = 0, int PageLimit = 25)
        {
            return await Task.Run<IActionResult>(() =>
            {
                var searchQuery = SearchText.ToUpper().Split(' ', StringSplitOptions.TrimEntries);
                var query = from a in _DbContext.Article
                            join p in _DbContext.Person on a.PERSONID equals p.ID
                            join u in _DbContext.User on p.USERID equals u.ID
                            join uo in _DbContext.UserOptions on p.USERID equals uo.USERID
                            join c in _DbContext.Currency on a.CURRENCYID equals c.ID
                            select new { ARTICLE = a, PERSON = p, USER = u, USEROPTIONS = uo, CURRENCY = c };

                // Get page count
                int pageCount = (int)Math.Ceiling((double)query.Count() / PageLimit);
                if (Page > pageCount)
                    Page = pageCount;

                // Get page
                if (Page > 0)
                    query = query.Skip((Page - 1) * PageLimit);

                query = query.Take(PageLimit);

                var queryResult = query.ToList();
                List<int> indexes = new List<int>();

                // Search by title, tags, person, and description
                for (int i = 0; i < queryResult.Count; ++i)
                {
                    var item = queryResult[i];
                    string title = item.ARTICLE.TITLE.ToUpper();
                    string tags = item.ARTICLE.TAGS.ToUpper();
                    string person = item.PERSON.DISPLAYNAME.ToUpper();
                    string description = item.ARTICLE.DESCRIPTION.ToUpper();

                    if (searchQuery.Any(s => title.Contains(s) || tags.Contains(s) || person.Contains(s) || description.Contains(s) || Utils.IsSimilar(title, s) || Utils.IsSimilar(person, s)))
                        indexes.Add(i);
                }

                if (indexes.Count == 0)
                    return Ok(TRESPONSE.OK(Request, indexes, LangManager.GetTranslationFromRequest("NO_ARTICLE_FOUND", Request)));

                var result = new ShopSearchResult[indexes.Count];
                for (int i = 0; i < indexes.Count; ++i)
                {
                    var item = queryResult[indexes[i]];
                    var person = PersonManager.MakePersonView(item.PERSON, item.USER, item.USEROPTIONS);

                    result[i] = new ShopSearchResult { ARTICLE = item.ARTICLE, PERSON = person, CURRENCY = item.CURRENCY };
                }

                object res = result;
                switch (OrderBy)
                {
                    case 0:
                        res = result.OrderByDescending(a => a.ARTICLE.CREATED);
                        break;
                    case 1:
                        res = result.OrderBy(a => a.ARTICLE.CREATED);
                        break;
                    case 2:
                        res = result.OrderByDescending(a => a.ARTICLE.VIEWS);
                        break;
                    case 3:
                        res = result.OrderBy(a => a.ARTICLE.PRICE);
                        break;
                    case 4:
                        res = result.OrderByDescending(a => a.ARTICLE.PRICE);
                        break;
                    case 5:
                        res = result.OrderBy(a => a.ARTICLE.QUANTITY);
                        break;
                    case 6:
                        res = result.OrderByDescending(a => a.ARTICLE.QUANTITY);
                        break;
                    case 7:
                        res = result.OrderBy(a => a.ARTICLE.TITLE);
                        break;
                    case 8:
                        res = result.OrderByDescending(a => a.ARTICLE.TITLE);
                        break;
                    case 9:
                        res = result.OrderBy(a => a.ARTICLE.VIEWS);
                        break;
                    case 10:
                        res = result.OrderByDescending(a => a.ARTICLE.VIEWS);
                        break;
                }

                return Ok(TRESPONSE.OK(Request, new { PAGES = pageCount, ARTICLES = res }, result.Length == 0 ? "NO_ARTICLE_FOUND" : string.Empty));

            });
        }

        [HttpGet("Suggestions")]
        [Authorize]
        // @param BasedOn = 0: Based on user's interests (Purchases and views), 1: Based on user's purchases, 2: Based on user's views
        public async Task<IActionResult> GetSuggestions(int BasedOn = 0, int Limit = 25)
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (Limit <= 0)
                    return BadRequest(TRESPONSE.ERROR(Request, "Limit must be greater than 0"));

                var userString = User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.UserData);
                if (userString is null)
                    return BadRequest(TRESPONSE.ERROR(Request, "INVALID_USER"));

                TUSER_V? user = JsonConvert.DeserializeObject<TUSER_V>(userString.Value);
                if (user is null)
                    return BadRequest(TRESPONSE.ERROR(Request, "INVALID_USER"));

                var recentViews = user.RECENT_VIEWD_ARTICLES.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var recentPurchases = user.RECENT_PURCHASED_ARTICLES.Split(',', StringSplitOptions.RemoveEmptyEntries);

                var query = from a in _DbContext.Article
                            join p in _DbContext.Person on a.PERSONID equals p.ID
                            join u in _DbContext.User on p.USERID equals u.ID
                            join uo in _DbContext.UserOptions on p.USERID equals uo.USERID
                            join c in _DbContext.Currency on a.CURRENCYID equals c.ID
                            where a.QUANTITY > 0
                            select new { ARTICLE = a, PERSON = p, USER = u, USEROPTIONS = uo, CURRENCY = c };

                var queryResult = query.ToList();
                Dictionary<string, int> searchTags = new Dictionary<string, int>();

                if (BasedOn == 0 || BasedOn == 1)
                {
                    // Based on user's purchases
                    for (int i = 0; i < recentPurchases.Length; ++i)
                    {
                        var item = queryResult.FirstOrDefault(a => a.ARTICLE.ID == int.Parse(recentPurchases[i]));
                        if (item is null)
                            continue;

                        string[] tags = item.ARTICLE.TAGS.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < tags.Length; ++j)
                        {
                            if (searchTags.ContainsKey(tags[j]))
                                searchTags[tags[j]]++;
                            else
                                searchTags.Add(tags[j], 1);
                        }
                    }
                }

                if (BasedOn == 0 || BasedOn == 2)
                {
                    // Based on user's views
                    for (int i = 0; i < recentViews.Length; ++i)
                    {
                        var item = queryResult.FirstOrDefault(a => a.ARTICLE.ID == int.Parse(recentViews[i]));
                        if (item is null)
                            continue;

                        string[] tags = item.ARTICLE.TAGS.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < tags.Length; ++j)
                        {
                            if (searchTags.ContainsKey(tags[j]))
                                searchTags[tags[j]]++;
                            else
                                searchTags.Add(tags[j], 1);
                        }
                    }
                }

                if (searchTags.Count == 0)
                {
                    // Get the most popular articles
                    var popularArticles = queryResult.OrderByDescending(a => a.ARTICLE.VIEWS).Take(Limit).ToArray();

                    var r = new ShopSearchResult[popularArticles.Length];
                    for (int i = 0; i < popularArticles.Length; ++i)
                    {
                        var item = popularArticles[i];
                        var person = PersonManager.MakePersonView(item.PERSON, item.USER, item.USEROPTIONS);

                        r[i] = new ShopSearchResult { ARTICLE = item.ARTICLE, PERSON = person, CURRENCY = item.CURRENCY };
                    }

                    return Ok(TRESPONSE.OK(Request, r));
                }

                // Collect every article that has at least one tag in common with the user's purchases and views
                Dictionary<int, int> indexes = new Dictionary<int, int>();
                for (int i = 0; i < queryResult.Count; ++i)
                {
                    var item = queryResult[i];

                    int count = 0;
                    string[] tags = item.ARTICLE.TAGS.Split(',', StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < tags.Length; ++j)
                    {
                        if (searchTags.TryGetValue(tags[j], out int val))
                            count += val;
                    }

                    if (count > 0)
                        indexes.Add(i, count);
                }

                // If no article has been found, return an empty list
                if (indexes.Count == 0)
                    return Ok(TRESPONSE.OK(Request, new ShopSearchResult[0], "NO_ARTICLE_FOUND"));

                // Sort by value (count)
                var sortedIndexes = indexes.OrderByDescending(a => a.Value);

                // Get the articles with the closest tags to the user's purchases and views
                int sortetIndexesCount = sortedIndexes.Count();
                var result = new ShopSearchResult[sortetIndexesCount < Limit ? sortetIndexesCount : Limit];
                for (int i = 0; i < result.Length; ++i)
                {
                    var item = queryResult[sortedIndexes.ElementAt(i).Key];
                    var person = PersonManager.MakePersonView(item.PERSON, item.USER, item.USEROPTIONS);

                    result[i] = new ShopSearchResult { ARTICLE = item.ARTICLE, PERSON = person, CURRENCY = item.CURRENCY };
                }

                return Ok(TRESPONSE.OK(Request, result));
            });
        }

        [HttpGet("Info")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleInfo(int ArticleId)
        {
            ArticleInformation info = new ArticleInformation();

            info.ARTICLEID = ArticleId;
            info.NUMIMAGES = await _DbContext.Image.CountAsync(e => e.ENTITYTYPE == "TARTICLE" && e.ENTITYID == ArticleId);

            return Ok(TRESPONSE.OK(Request, info));
        }

        [HttpGet("Image")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArticleImage(int ArticleId, int ImageIndex = 0)
        {
            var result = await ModelManager.GetImage<TARTICLE>(ArticleId, _DbContext.Image, ImageIndex);
            if (result is null)
                return NotFound();

            return File(result.DATA, "image/" + result.TYPE);
        }

        [HttpPost("Image")]
        [Authorize]
        public async Task<IActionResult> DeleteArticleImage(int ArticleId, int ImageIndex = 0)
        {
            int userId = int.Parse(User.Claims.First(e => e.Type == ClaimTypes.NameIdentifier).Value);
            int personId = _DbContext.Person.First(e => e.USERID == userId).ID;

            var article = await _DbContext.Article.FirstOrDefaultAsync(e => e.ID == ArticleId && e.PERSONID == personId);
            if (article is null)
                return NotFound(TRESPONSE.ERROR(Request, "Article not found", false));

            if (await ModelManager.DeleteImage<TARTICLE>(ArticleId, _DbContext.Image, ImageIndex))
                return Ok(TRESPONSE.OK(Request));

            return BadRequest(TRESPONSE.ERROR(Request, "UNKOWN_ERROR"));
        }
    }
}