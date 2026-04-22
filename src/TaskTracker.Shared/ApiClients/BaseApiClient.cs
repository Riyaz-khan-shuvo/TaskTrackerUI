using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Shared.ApiClients
{
    public abstract class BaseApiClient
    {
        protected readonly string BaseUrl;
        protected readonly HttpClient HttpClient;
        public static Func<string> GetTokenFromSession { get; set; }

        //protected BaseApiClient(HttpClient httpClient, IConfiguration configuration)
        //{
        //    HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        //    HttpClient.Timeout = TimeSpan.FromSeconds(60);
        //    BaseUrl = configuration?["ApiSettings:BaseUrl"]
        //              ?? throw new ArgumentNullException("BaseUrl is missing in configuration");
        //}

        protected BaseApiClient()
        {
            HttpClient = SingletonHttpClient.Instance;
            HttpClient.Timeout = TimeSpan.FromSeconds(60);
            BaseUrl = "https://localhost:7025/";
            AddAuthorizationHeader();
        }

        private class SingletonHttpClient
        {
            public static readonly HttpClient Instance = new HttpClient();
        }
        protected void AddAuthorizationHeader()
        {
            HttpClient.DefaultRequestHeaders.Authorization = null;

            var token = GetTokenFromSession?.Invoke();
            if (!string.IsNullOrEmpty(token))
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

}










//using Microsoft.Extensions.Configuration;

//namespace TaskTracker.Repo.ApiClients
//{
//    public abstract class BaseApiClient
//    {
//        protected readonly string BaseUrl;
//        protected readonly HttpClient HttpClient;

//        // DI-compatible constructor
//        protected BaseApiClient(HttpClient httpClient, IConfiguration configuration)
//        {
//            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
//            HttpClient.Timeout = TimeSpan.FromSeconds(60);
//            BaseUrl = configuration?["ApiSettings:BaseUrl"]
//                      ?? throw new ArgumentNullException("BaseUrl is missing in configuration");
//        }

//        protected BaseApiClient()
//        {
//            HttpClient = SingletonHttpClient.Instance;
//            HttpClient.Timeout = TimeSpan.FromSeconds(60);
//            BaseUrl = "https://api.example.com/";
//        }

//        private class SingletonHttpClient
//        {

//            public static readonly HttpClient Instance = new HttpClient();
//        }

//        // Helper to combine URLs
//        protected string CombineUrl(string relativeUrl)
//        {
//            if (string.IsNullOrWhiteSpace(relativeUrl))
//                throw new ArgumentNullException(nameof(relativeUrl));

//            return $"{BaseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
//        }
//    }
//}
