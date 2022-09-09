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
        public static HttpClient? _accountsApi { get; set; }
        public static HttpClient? _mediaApi { get; set; }

        public static string WebApiUrl { get; set; } = "https://localhost:9595/api/";

        public static Guid? _userId { get; set; }
        public static string? _apiToken { get; set; }
        public static Account? _currentUser { get; set; }
        public static List<Account> _allUsers { get; set; } = new List<Account>();
        public static MainWindow? _mainWindow { get; set; }
        public static LoginWindow? _loginWindow { get; set; }
        public static List<Media> _mediaList { get; set; } = new List<Media>();
        public static Watchlist? Watchlist { get; set; }
        public static Watchlist? _userWatchlist { get; set; }
        public static AccountLog? _userAccountLog { get; set; }
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

        public static async void GetUserWatchlist(Account user)
        {

            _mediaApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
            string path = "users/" + user.Id.ToString() + "/watchlists";

            try
            {
                HttpResponseMessage response = await _mediaApi.GetAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("GetWatchlist:" + response.StatusCode);
                    return;
                }

                var content = await response.Content.ReadAsStringAsync();

                _userWatchlist = JArray.Parse(content).ToObject<List<Watchlist>>()[0];
                Console.WriteLine("Watchlist loaded: " + _userWatchlist.Id + ": " + _userWatchlist.Name);

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

        public static async Task<string> AddNewUser(RegisterRequest registerRequest)
        {
            string message;
            string path = "";

            _accountsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            var authenticateRequestContent = JsonConvert.SerializeObject(registerRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await _accountsApi.PostAsync(path, httpContent);
                if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.BadRequest)
                {
                    message = "ERROR in AddNewUser: " + response.StatusCode;
                    return message;
                }

                var content = await response.Content.ReadAsStringAsync();
                JObject joResponse = JObject.Parse(content);

                if ((bool)joResponse["status"] && joResponse["token"] != null && joResponse["id"] != null)
                {
                    message = "Success";
                    GetAllUsers();
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

                Console.WriteLine(_currentUser.Type);
                if (_currentUser.Type == AccountType.Admin)
                {
                    GetAllUsers();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SetCurrentUser: " + ex.Message);
            }
        }

        public static async Task<bool> GetAllUsers()
        {
            _accountsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);

            HttpResponseMessage response = await _accountsApi.GetAsync("");


            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error in GetAllUsers:" + response.StatusCode);
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();

            try
            {
                _allUsers = JArray.Parse(content).ToObject<List<Account>>();
                Console.WriteLine(_allUsers.Count);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GetAllUsers: " + ex.Message);
            }

            return true;

        }

        public static void Logout()
        {
            InitAccountsApi();
            _currentUser = null;
            _apiToken = null;
            _userId = null;
            _accountsApi.DefaultRequestHeaders.Authorization = null;
            _mediaApi.DefaultRequestHeaders.Authorization = null;
            Watchlist = null;

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
        public static async void DeleteSelectedUser(Account user)
        {
            Console.WriteLine("Removing User(" + user.Id + "): " + user.Username);
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                string path = user.Id.ToString();
                Console.WriteLine("Delete from: " + path);
                HttpResponseMessage response = await _accountsApi.DeleteAsync(path);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("DeleteSelectedUser:" + response.StatusCode);
                    Mouse.OverrideCursor = null;
                    return;
                }
                var content = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(content).ToObject<DeleteResponse>();

                if (result.Status)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Successful DeleteSelectedUser: " + content);
                    var status = await GetAllUsers();
                }
                else
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("User was not deleted: " + result.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteSelectedUser: " + ex.Message);
                Mouse.OverrideCursor = null;

            }
        }
        public static async void DeleteCurrentUser()
        {
            if (_currentUser is not null)
            {
                Console.WriteLine("Removing Current User(" + _currentUser.Id + "): " + _currentUser.Username);
                Mouse.OverrideCursor = Cursors.Wait;

                try
                {
                    string path = "session";
                    Console.WriteLine("Delete from: " + path);
                    HttpResponseMessage response = await _accountsApi.DeleteAsync(path);

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("DeleteCurrentUser:" + response.StatusCode);
                        Mouse.OverrideCursor = null;
                        return;
                    }
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JObject.Parse(content).ToObject<DeleteResponse>();

                    if (result.Status)
                    {
                        Console.WriteLine("Successful DeleteCurrentUser: " + content);
                        Mouse.OverrideCursor = null;
                        Logout();
                    }
                    else
                    {
                        Mouse.OverrideCursor = null;
                        MessageBox.Show("User was not deleted: " + result.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in DeleteCurrentUser: " + ex.Message);
                    Mouse.OverrideCursor = null;

                }
            }

        }
        public static async void AdminUpdateSelectedUser(Account user, UpdateRequest updateRequest)
        {

            string path = user.Id.ToString();
            Console.WriteLine("Put to: " + path);
            Mouse.OverrideCursor = Cursors.Wait;

            var authenticateRequestContent = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _accountsApi.PutAsync(path, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error in UpdateUser:" + response.StatusCode);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(content).ToObject<UpdateResponse>();

            if (result.Status)
            {
                Console.WriteLine("Successful UpdateUser: " + content);
                Mouse.OverrideCursor = null;
                MessageBox.Show("User was Updated: " + result.Message);
                SetCurrentUser();
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("User was not updated: " + result.Message);
            }
            //string path = user.Id.ToString();
            //Console.WriteLine("Put to: " + path);
            //Mouse.OverrideCursor = Cursors.Wait;

            //UpdateRequest updateRequest = new UpdateRequest { FirstName = user.FirstName, LastName=user.LastName, Email=user.Email };

            //var authenticateRequestContent = JsonConvert.SerializeObject(updateRequest);
            //var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await _accountsApi.PutAsync("session", httpContent);

            //if (!response.IsSuccessStatusCode)
            //{
            //    MessageBox.Show("Error in UpdateCurrentuser:" + response.StatusCode);
            //    return;
            //}

            //var content = await response.Content.ReadAsStringAsync();
            //var result = JObject.Parse(content).ToObject<UpdateResponse>();

            //if (result.Status)
            //{
            //    Console.WriteLine("Successful UpdateCurrentuser: " + content);
            //    Mouse.OverrideCursor = null;
            //    MessageBox.Show("User was Updated: " + result.Message);
            //    SetCurrentUser();
            //}
            //else
            //{
            //    Mouse.OverrideCursor = null;
            //    MessageBox.Show("User was not updated: " + result.Message);
            //}

        }

        public static async void AddNewUser(Account user)
        {


            //if (user is not null)
            //{
            //    Console.WriteLine("Adding User: " + user.FirstName + " " + user.LastName);
            //    UpdateRequest UserUpdateRequest = new UpdateRequest { Id = user.Id, FirstName = user.FirstName, 
            //        LastName = user.LastName };
                


            //    var updateRequestContent = JsonConvert.SerializeObject(UserUpdateRequest);
            //    var httpContent = new StringContent(updateRequestContent, Encoding.UTF8, "application/json");

            //    try
            //    {
            //        string path = "session/watchlists/" + Watchlist.Id + "/entries";
            //        Console.WriteLine("Post to: " + path);
            //        HttpResponseMessage response = await _mediaApi.PostAsync(path, httpContent);

            //        if (!response.IsSuccessStatusCode)
            //        {
            //            MessageBox.Show("AddSelectedMediaToWatchlist:" + response.StatusCode);
            //            return false;
            //        }
            //        var content = await response.Content.ReadAsStringAsync();
            //        Console.WriteLine("Successful AddSelectedMediaToWatchlist: " + content);

            //        GetWatchlist();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Error in GetWatchlist: " + ex.Message);
            //    }
            //}
            //return true;
        }
        public static async void UpdateCurrentuser(UpdateRequest updateRequest)
        {
            string path = "session";
            Console.WriteLine("Put to: " + path);
            Mouse.OverrideCursor = Cursors.Wait;

            var authenticateRequestContent = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(authenticateRequestContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _accountsApi.PutAsync("session", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error in UpdateCurrentuser:" + response.StatusCode);
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(content).ToObject<UpdateResponse>();

            if (result.Status)
            {
                Console.WriteLine("Successful UpdateCurrentuser: " + content);
                Mouse.OverrideCursor = null;
                MessageBox.Show("User was Updated: " + result.Message);
                SetCurrentUser();
            }
            else
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show("User was not updated: " + result.Message);
            }
            
        }
    }

}
