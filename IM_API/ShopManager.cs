using IMAPI;

namespace IM_API
{
    public static class ShopManager
    {
        public static async Task<string> CreateArticle(IMDbContext DbContext, TARTICLE Model)
        {
            try
            {
                ModelManager.FillDefaults(Model, true);
                DbContext.Article.Add(Model);

                await DbContext.SaveChangesAsync();
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> CreateArticle(IMDbContext DbContext, TARTICLE Model, int UserId)
        {
            // Get person from user id
            var person = await DbContext.Person.FirstOrDefaultAsync(p => p.USERID == UserId);
            if (person == null)
                return "Person not found";

            // Create article
            Model.PERSONID = person.ID;
            return await CreateArticle(DbContext, Model);
        }

        public static async Task<string> UpdateArticle(IMDbContext DbContext, TARTICLE Model)
        {
            try
            {
                var article = await DbContext.Article.FirstOrDefaultAsync(a => a.ID == Model.ID);
                if (article == null)
                    return "Article not found";

                ModelManager.CopyModel(article, Model);
                await DbContext.SaveChangesAsync();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> UpdateArticle(IMDbContext DbContext, TARTICLE Model, int UserId)
        {
            // Get person from user id
            var person = await DbContext.Person.FirstOrDefaultAsync(p => p.USERID == UserId);
            if (person == null)
                return "Person not found";

            // Create article
            if(Model.PERSONID != person.ID)
                return "Article does not belong to this user";

            return await UpdateArticle(DbContext, Model);
        }

        public static async Task<string> ViewArticle(IMDbContext DbContext, int ArticleId, int UserId)
        {
            try
            {
                var article = await DbContext.Article.FirstOrDefaultAsync(a => a.ID == ArticleId);
                if (article == null)
                    return "Article not found";

                article.VIEWS++;
                await DbContext.SaveChangesAsync();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
