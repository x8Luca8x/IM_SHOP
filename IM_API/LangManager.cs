using Newtonsoft.Json;

namespace IM_API
{
    public static class LangManager
    {
        private static Dictionary<string, TLANG> Langs = new Dictionary<string, TLANG>();

        public static TLANG GetLangPacket(string Language)
        {
            if(string.IsNullOrWhiteSpace(Language))
                return new TLANG();

            if(Langs.TryGetValue(Language, out TLANG? ln))
                return ln;

            try
            {            
                // Read json file
                string json = File.ReadAllText($"Lang/{Language}.json");

                // Deserialize json
                TLANG? lang = JsonConvert.DeserializeObject<TLANG>(json);
                if (lang is null)
                    lang = new TLANG();

                // Add to dictionary
                Langs.Add(Language, lang);

                return lang;
            }
            catch (Exception)
            {
                return new TLANG();
            }
        }

        public static string GetTranslation(string String, string Language)
        {
            TLANG lang = GetLangPacket(Language);
            string value = lang.GetType().GetProperty(String)?.GetValue(lang)?.ToString() ?? String;

            return value;
        }

        public static string GetTranslationFromRequest(string String, HttpRequest Request)
        {
            string language = Request.Headers["Language"].ToString();
            bool bGetRaw = Request.Headers["GetRaw"].ToString() == "true";

            return bGetRaw ? String : GetTranslation(String, language);
        }
    }
}
