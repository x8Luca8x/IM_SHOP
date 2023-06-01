using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM_API
{
    public class TRESPONSE
    {
        TRESPONSE(bool success, dynamic? data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public bool Success { get; set; }
        public dynamic? Data { get; set; }
        public string Message { get; set; }

        public static TRESPONSE OK(HttpRequest Request, dynamic? data = null, string message = "", bool bTranslate = true)
        {
            string msg = bTranslate ? LangManager.GetTranslationFromRequest(message, Request) : message;
            return new TRESPONSE(true, data, msg);
        }

        public static TRESPONSE ERROR(HttpRequest Request, string message, bool bTranslate = true)
        {
            string msg = bTranslate ? LangManager.GetTranslationFromRequest(message, Request) : message;
            return new TRESPONSE(false, null, msg);
        }
    }
}
