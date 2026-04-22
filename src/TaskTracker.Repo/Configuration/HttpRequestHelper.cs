using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static TaskTracker.Models.CommonModel;

namespace TaskTracker.Repo.Configuration
{
    public class HttpRequestHelper
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HttpRequestHelper(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(60);

            // Read BaseUrl from appsettings.json
            _baseUrl = configuration["ApiSettings:BaseUrl"] 
                       ?? throw new ArgumentNullException("BaseUrl is missing in configuration");
        }

        // ✅ LOGIN
        public async Task<AuthModel> GetLoginAuthenticationAsync(CredentialModel credentialModel)
        {
            return await PostAsync<AuthModel>("api/UserLogin/SignIn", credentialModel);
        }

        public async Task<AuthModel> GetAuthenticationAsync(CredentialModel credentialModel)
        {
            return await PostAsync<AuthModel>("api/Auth/login", credentialModel);
        }

        // ✅ Generic POST request
        //public async Task<T> PostAsync<T>(string url, object payload, string token = null)
        //{
        //    string fullUrl = CombineUrl(url);

        //    var json = JsonSerializer.Serialize(payload);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    if (!string.IsNullOrEmpty(token))
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    using var response = await _httpClient.PostAsync(fullUrl, content);
        //    response.EnsureSuccessStatusCode();

        //    var resultString = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}


        public async Task<T> PostAsync<T>(string url, object payload, string token = null)
        {
            string fullUrl = CombineUrl(url);

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Clear previous Authorization header (so token can be skipped safely)
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.PostAsync(fullUrl, content);

            // Handle forbidden (403)
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                var res = await response.Content.ReadAsStringAsync();

                // Return the API response exactly
                return JsonSerializer.Deserialize<T>(
                    res,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }

            // Other errors—still let API message reach UI
            if (!response.IsSuccessStatusCode)
            {
                var errorString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(
                    errorString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<T> PostFormAsync<T>(string url, MultipartFormDataContent formData, string token = null)
        {
            string fullUrl = CombineUrl(url);

            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.PostAsync(fullUrl, formData);
            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // ✅ Generic GET request
        public async Task<T> GetAsync<T>(string url, string token = null)
        {
            string fullUrl = CombineUrl(url);

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.GetAsync(fullUrl);
            response.EnsureSuccessStatusCode();

            var resultString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(resultString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // ✅ For downloading reports
        public async Task<Stream> PostDataReportAsync(string url, object payload, string token)
        {
            string fullUrl = CombineUrl(url);

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(fullUrl, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        // ✅ Helper: safely combine base URL and relative path
        private string CombineUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                throw new ArgumentNullException(nameof(relativeUrl));

            return $"{_baseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
        }
    }
}
