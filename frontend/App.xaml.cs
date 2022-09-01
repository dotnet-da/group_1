using AccountsDLL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient _accountsApi { get; set; }

        public static Guid _userId { get; set; }
        public static string _apiToken { get; set; }

        public App()
        {
            // Get data from accounts API
            _accountsApi = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            _accountsApi.DefaultRequestHeaders.Accept.Clear();
            _accountsApi.DefaultRequestHeaders.Accept.Add(
                                 new MediaTypeWithQualityHeaderValue("application/json"));
            _accountsApi.BaseAddress = new Uri("https://localhost:9595/api/accounts/");
            //_accountsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public static async Task<string> Login(string username, string password)
        {
            string message = "";
            string path = "authenticate";

            AuthenticateRequest authenticateRequest = new AuthenticateRequest
            {
                Username = username,
                Password = password
            };

            var authenticateRequestContent = JsonConvert.SerializeObject(authenticateRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _accountsApi.PostAsync(path, httpContent);

            if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                message = "ERROR in login: " + response.StatusCode;
                return message;
            }

            var content = await response.Content.ReadAsStringAsync();
            JObject joResponse = JObject.Parse(content);

            if (joResponse["token"] != null && joResponse["id"] != null)
            {
                message = "Success";
                _userId = (Guid)joResponse["id"];
                _apiToken = (string)joResponse["token"];
            }
            else
            {
                message = (string)joResponse["message"];
            }

            return message;
        }
    }
}
