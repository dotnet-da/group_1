using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StreamKing.Data.Accounts;
using StreamKing.Data.Media;
using StreamKing.Login;
using StreamKing.MainApplication;
using StreamKing.Web.Models;
using System;
using System.Collections.Generic;
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
        public static HttpClient _mediaApi { get; set; }

        public static string WebApiUrl { get; set; } = "https://localhost:9595/api/";

        public static Guid? _userId { get; set; }
        public static string? _apiToken { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImRmNDZhMWMyLWY5ZmYtNDUwOC05OWQwLTg4NmVlNGY2NzUzYyIsImF1dGgiOiJBZG1pbiIsIm5iZiI6MTY2MjU0NTM4OSwiZXhwIjoxNjYzMTUwMTg5LCJpYXQiOjE2NjI1NDUzODl9.OJeI8L9S91EOrUwjSGUhwy5lxhIglrJEvQWd5VNnAe0";
        public static Account? _currentUser { get; set; }
        public static MainWindow? _mainWindow { get; set; }
        public static LoginWindow? _loginWindow { get; set; }
        public static List<Media> _mediaList { get; set; } = new List<Media>();
        public static Watchlist? Watchlist { get; set; }
        public App()
        {
            InitAccountsApi();
            InitMediaApi();
            LoadMedia("?type=movie&take=50");
            LoadMedia("?type=series&take=50");
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
            _accountsApi.BaseAddress = new Uri(WebApiUrl + "accounts/");

        }

        public static async void LoadMedia(string url)
        {
            HttpClient tempApi = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            tempApi.DefaultRequestHeaders.Accept.Clear();
            tempApi.DefaultRequestHeaders.Accept.Add(
                                 new MediaTypeWithQualityHeaderValue("application/json"));
            tempApi.BaseAddress = new Uri(WebApiUrl + "media/");

            try
            {
                HttpResponseMessage response = await tempApi.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error in LoadMedia:" + response.StatusCode);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    if (url.Contains("movie"))
                    {
                        var movieList = JArray.Parse(content).ToObject<List<Movie>>();
                        _mediaList.AddRange(movieList);
                        if (_mainWindow != null)
                        {
                            _mainWindow.SetMedialist();
                        }
                    }
                    else if (url.Contains("series"))
                    {
                        var seriesList = JArray.Parse(content).ToObject<List<Series>>();
                        _mediaList.AddRange(seriesList);
                        if (_mainWindow != null)
                        {
                            _mainWindow.SetMedialist();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in LoadMedia Parsing: " + ex.Message);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LoadMedia: " + ex.Message);
            }
        }

        public static void InitMediaApi()
        {
            _mediaApi = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            _mediaApi.DefaultRequestHeaders.Accept.Clear();
            _mediaApi.DefaultRequestHeaders.Accept.Add(
                                 new MediaTypeWithQualityHeaderValue("application/json"));
            _mediaApi.BaseAddress = new Uri(WebApiUrl + "media/");
        }

        public static async void GetWatchlist()
        {
            _mediaApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            try
            {
                HttpResponseMessage response = await _mediaApi.GetAsync("session/watchlists");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("GetWatchlist:" + response.StatusCode);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();

                Watchlist = JArray.Parse(content).ToObject<List<Watchlist>>()[0];
                Console.WriteLine("Watchlist loaded: " + Watchlist.Id + ": " + Watchlist.Name);

                if (_mainWindow != null)
                {
                    _mainWindow.SetWatchlist();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetWatchlist: " + ex.Message);
            }
        }
        public static async Task<bool> AddSelectedMediaToWatchlist()
        {
            Media? selected = _mainWindow.GetSelectedMedia();
            if (selected is not null && Watchlist is not null)
            {
                Console.WriteLine("Adding to Watchlist(" + Watchlist.Id + "): " + selected.Title);
                WatchEntryRequest watchEntryRequest = new WatchEntryRequest
                {
                    Tag = "Watching",
                };

                if (selected.GetType() == typeof(Movie))
                {
                    watchEntryRequest.MovieId = selected.TmdbId;
                }
                else if (selected.GetType() == typeof(Series))
                {
                    watchEntryRequest.SeriesId = selected.TmdbId;
                }

                var watchEntryRequestContent = JsonConvert.SerializeObject(watchEntryRequest);
                var httpContent = new StringContent(watchEntryRequestContent, Encoding.UTF8, "application/json");

                try
                {
                    string path = "session/watchlists/" + Watchlist.Id + "/entries";
                    Console.WriteLine("Post to: " + path);
                    HttpResponseMessage response = await _mediaApi.PostAsync(path, httpContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("AddSelectedMediaToWatchlist:" + response.StatusCode);
                        return false;
                    }
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successful AddSelectedMediaToWatchlist: " + content);

                    GetWatchlist();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetWatchlist: " + ex.Message);
                }
            }
            return true;
        }

        public static async Task<bool> RemoveSelectedWatchEntryFromWatchlist()
        {
            WatchEntry? selected = _mainWindow.GetSelectedWatchEntry();
            if (selected is not null && Watchlist is not null)
            {
                Console.WriteLine("Removing from Watchlist(" + Watchlist.Id + "): " + selected.Id);

                try
                {
                    string path = "session/watchlists/" + Watchlist.Id + "/entries/" + selected.Id;
                    Console.WriteLine("Delete from: " + path);
                    HttpResponseMessage response = await _mediaApi.DeleteAsync(path);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("RemoveSelectedWatchEntryFromWatchlist:" + response.StatusCode);
                        return false;
                    }
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successful RemoveSelectedWatchEntryFromWatchlist: " + content);

                    GetWatchlist();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in RemoveSelectedWatchEntryFromWatchlist: " + ex.Message);
                }
            }
            return true;
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

                    Console.WriteLine(_apiToken);

                    Mouse.OverrideCursor = null;
                    _mainWindow = new MainWindow();
                    _mainWindow.Show();

                    SetCurrentUser();
                    GetWatchlist();
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

        public static async Task<string> Register(RegisterRequest registerRequest)
        {
            string message;
            string path = "";

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

                    // Clear the model after succesfully signed up
                    LoginWindow.ClearUserModel();
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
                _userId = _currentUser.Id;
                _mainWindow.UpdateHeader();
                _mainWindow.UpdateCurrentUser();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SetCurrentUser: " + ex.Message);
            }
        }

        public static void Logout()
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

        public static async void SwitchRegion(string region)
        {
            UpdateRequest updateRequest = new UpdateRequest
            {
                Region = region,
            };

            var authenticateRequestContent = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _accountsApi.PutAsync("session", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error in SwitchRegion:" + response.StatusCode);
                return;
            }
        }
    }
}
