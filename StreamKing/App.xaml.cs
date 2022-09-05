using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StreamKing.Data.Accounts;
using StreamKing.Login;
using StreamKing.MainApplication;
using StreamKing.Web.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StreamKing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient _accountsApi { get; set; }

        public static Guid? _userId { get; set; }
        public static string? _apiToken { get; set; }
        public static Account? _currentUser { get; set; }
        public static MainWindow? _mainWindow { get; set; }
        public static LoginWindow? _loginWindow { get; set; }
        public App()
        {
            InitAccountsApi();
        }

        public static void InitAccountsApi()
        {
            _accountsApi = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            _accountsApi.DefaultRequestHeaders.Accept.Clear();
            _accountsApi.DefaultRequestHeaders.Accept.Add(
                                 new MediaTypeWithQualityHeaderValue("application/json"));
            _accountsApi.BaseAddress = new Uri("https://localhost:9595/api/accounts/");
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
            try
            {
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

                    Mouse.OverrideCursor = null;
                    _mainWindow = new MainWindow();
                    _mainWindow.Show();

                    SetCurrentUser();
                }
                else
                {
                    message = (string)joResponse["message"];
                }
            }
            catch (Exception ex)
            {
                message = "ERROR:\n" + ex.Message;
            }

            return message;
        }

        public static async Task<string> Register(string username, string password)
        {
            string message;
            string path = "";

            RegisterRequest registerRequest = new RegisterRequest
            {
                Username = username,
                Password = password
            };

            var authenticateRequestContent = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _accountsApi.PostAsync(path, httpContent);
                if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    message = "ERROR in sign up: " + response.StatusCode;
                    return message;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                if ((bool)joResponse["status"] && joResponse["token"] != null && joResponse["id"] != null)
                {
                    message = "Success";
                    _userId = (Guid)joResponse["id"];
                    _apiToken = (string)joResponse["token"];
                }
                else
                {
                    message = (string)joResponse["message"];
                }
            }
            catch (Exception ex)
            {
                message = "ERROR:\n" + ex.Message;
            }

            return message;
        }

        public static async void SetCurrentUser()
        {
            _accountsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            HttpResponseMessage response = await _accountsApi.GetAsync("session");


            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error in SetCurrentUser:" + response.StatusCode);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                _currentUser = JObject.Parse(content).ToObject<Account>();
                _mainWindow.UpdateHeader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SetCurrentUser: " + ex.Message);
            }
        }

        public static async void Logout()
        {
            InitAccountsApi();
            _currentUser = null;
            _apiToken = null;
            _userId = null;

            _loginWindow = new LoginWindow();
            _loginWindow.Show();

            if (_mainWindow != null)
            {
                _mainWindow.Close();
            }
            _mainWindow = null;
        }
    }
}
