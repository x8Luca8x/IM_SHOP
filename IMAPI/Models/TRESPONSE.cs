using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI.Models
{
    public class TRESPONSE
    {
        TRESPONSE(bool success, dynamic data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public bool Success { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }

        public static TRESPONSE OK(dynamic data = null, string message = "")
        {
            return new TRESPONSE(true, data, message);
        }

        public static TRESPONSE ERROR(string message)
        {
            return new TRESPONSE(false, null, message);
        }
    }
}
