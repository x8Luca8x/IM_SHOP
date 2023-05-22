using IMAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IMAPI
{
    public enum EAuthResult
    {
        OK,
        INVALID_TOKEN,
        INACTIVE_USER,
        NOT_VERIFIED_USER,
        TOKEN_EXPIRED,
        UNKNOWN_ERROR
    }

    public struct TAUTH
    {
        public readonly TUSER_V? user { get; }
        public readonly EAuthResult result { get; }

        public bool IsOK()
        {
            return result == EAuthResult.OK && user is not null;
        }

        public TAUTH(TUSER_V? user, EAuthResult result)
        {
            this.user = user;
            this.result = result;
        }
    }

    public struct TDEVICE
    {
        public TDEVICE()
        {
            name = "";
            os = "";
            app = "";
        }

        public TDEVICE(string JsonString)
        {
            var obj = JsonConvert.DeserializeObject<TDEVICE>(JsonString);

            name = obj.name;
            os = obj.os;
            app = obj.app;
        }

        public string name { get; set; }
        public string os { get; set; }
        public string app { get; set; }
    }

    public struct TAUTHENTICATION
    {
        public string TOKEN { get; set; }
        public TDEVICE DEVICE { get; set; }
    }

    public static class IMAPI
    {
#if DEBUG
        public static readonly string API_URL = "https://localhost:8081/api/";
#else
        public static readonly string API_URL = "https://imapi.azurewebsites.net/api/";
#endif

        private static HttpClient? _httpClient = null;

        public static HttpClient GetHttpClient()
        {
            if (_httpClient is null)
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(API_URL);
            }

            return _httpClient;
        }

        public static async Task<TAUTH> Authenticate(string token, TDEVICE Device)
        {
            HttpClient client = GetHttpClient();

            TAUTHENTICATION model = new TAUTHENTICATION();

            model.TOKEN = token;
            model.DEVICE = Device;

            string content = JsonConvert.SerializeObject(model);
            var response = await client.PostAsync("Authenticate", new StringContent(content));

            string responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonConvert.DeserializeObject<TAUTH>(responseContent);
                return result;
            }
            catch (Exception)
            {
                return new TAUTH(null, EAuthResult.UNKNOWN_ERROR);
            }
        }
    }
}
