using System.Net.Http;
using System.Text;
using System.Web;
using Formatting = Newtonsoft.Json.Formatting;
using System.IO;
using System.Threading.Tasks;
using NitsoAsset.Assets;
using System;
using NitsoAsset.Assets.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ModernHttpClient;
using System.Net;
using System.Linq;
using Autofac;
using NitsoAsset.ViewModels;

namespace NitsoAsset.Services.Data
{
    public static class ProxyBase<T> where T : class
    {
        public static string Headers { get; set; }

        static bool AutoLogin = true;

        public static async Task<T> Delete(string url, bool handleDoubleDeserialize = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Settings.ServiceBaseURL);
                    url = $"{client.BaseAddress}{url}";

                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Settings.ServiceToken);
                    client.DefaultRequestHeaders.Add("authorization", "Bearer " + Settings.ServiceToken);

                    var response = await client.DeleteAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseJson)) //(responseJson.IsEmpty())
                        {
                            return default(T);
                        }
                        T result;
                        if (handleDoubleDeserialize)
                        {
                            result = JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject<string>(responseJson));
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<T>(responseJson);
                        }

                        return result;
                    }

                    return default(T);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<T> Get(string url, bool requiresToken = true, bool handleDoubleDeserialize = false)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!url.StartsWith("http"))
                    {
                        client.BaseAddress = new Uri(Settings.ServiceBaseURL);
                        url = $"{client.BaseAddress}{url}";
                    }

                    if (requiresToken)
                    {
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Settings.ServiceToken);
                        client.DefaultRequestHeaders.Add("authorization", "Bearer " + Settings.ServiceToken);
                    }
                    System.Diagnostics.Debug.WriteLine("\n------------------------");
                    System.Diagnostics.Debug.WriteLine($"GET API Url : {url}");
                    var response = await client.GetAsync(url);

                    var response1 = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"GET API StatusCode : {response.StatusCode}");
                        System.Diagnostics.Debug.WriteLine("------------------------\n");
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseJson))//(responseJson.IsEmpty())
                        {
                            return default(T);
                        }
                        T result;
                        if (handleDoubleDeserialize)
                        {
                            result = JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject<string>(responseJson));
                        }
                        else
                        {
                            result = JsonConvert.DeserializeObject<T>(responseJson);
                        }

                        return result;
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<T> Post(string url, object postData, bool requiresToken = true, bool encode = true, bool handleDoubleDeserialize = false)
        {
            var query = string.Empty;
            // convert the post data to a url query string
            // example: param1=value1&param2=value2&parame3=value3
            var postString = JsonConvert.SerializeObject(postData, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            System.Diagnostics.Debug.WriteLine("\n------------------------");
            System.Diagnostics.Debug.WriteLine($"POST API Url : {Settings.ServiceBaseURL}{url}");
            System.Diagnostics.Debug.WriteLine($"POST API Request : {postString}");

            if (encode)
            {
                var jObj = (JObject)JsonConvert.DeserializeObject(postString);
                query = String.Join("&", jObj.Children().Cast<JProperty>().Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));
            }
            else
            {
                query = postString;
            }

            return await PostOrPut(url, query, HttpMethod.Post, requiresToken, encode, handleDoubleDeserialize);
        }

        private static async Task<T> PostOrPut(string url, string postString, HttpMethod method, bool requiresToken, bool encode = true, bool handleDoubleDeserialize = false)
        {
            try
            {
                using (var client = new HttpClient(new NativeMessageHandler() { Timeout = new TimeSpan(0, 2, 0), DisableCaching = true }))
                {
                    //var cancellationTokenSrc = new CancellationTokenSource(15000);
                    //var cancellationToken = cancellationTokenSrc.Token;

                    if (!url.StartsWith("http"))
                    {
                        client.BaseAddress = new Uri(Settings.ServiceBaseURL);
                        url = $"{client.BaseAddress}{url}";
                    }

                    StringContent postContent = null;

                    if (encode)
                    {
                        postContent = new StringContent(postString, Encoding.UTF8, "application/x-www-form-urlencoded");
                    }
                    else
                    {
                        postContent = new StringContent(postString, Encoding.UTF8, "application/json");
                    }
                    //postContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");
                    //postContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    //postContent.Headers.ContentEncoding.Add(Encoding.UTF8.ToString());

                    if (requiresToken && !string.IsNullOrEmpty(Settings.ServiceToken) && Settings.ServiceToken != " ")
                    {
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Settings.ServiceToken);
                        client.DefaultRequestHeaders.Add("authorization", Settings.ServiceToken);
                    }

                    //client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = new HttpResponseMessage();

                    if (method == HttpMethod.Post)
                    {
                        response = await client.PostAsync(url, postContent);
                    }
                    else
                    {
                        response = await client.PutAsync(url, postContent);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !url.Contains(AppConst.UserLoginUrl))
                    {
                        var _viewModel = App.Container.Resolve<DrawerPageViewModel>();
                        if (_viewModel != null)
                        {
                            //_viewModel.Logout();
                            return default(T);
                        }

                    }

                    var data = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"POST API StatusCode : {response.StatusCode}");
                        System.Diagnostics.Debug.WriteLine("------------------------\n");

                        var responseJson = string.Empty;

                        try
                        {
                            responseJson = await response.Content.ReadAsStringAsync();

                            if (string.IsNullOrEmpty(responseJson))//(responseJson.IsEmpty())
                            {
                                return default(T);
                            }
                            ////////
                            /*
                            if ((url.Contains(AppConst.LoginApiUrl) || url.Contains(AppConst.PasswordRecoveryApiUrl)) && response.Headers != null && response.Headers.Contains("Token"))
                            {
                                string token = response.Headers.GetValues("Token").FirstOrDefault();

                                Settings.ServiceToken = token;
                            }
                            */
                            ////////
                            T result;
                            if (handleDoubleDeserialize)
                            {
                                result = JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject<string>(responseJson));
                            }
                            else
                            {
                                result = JsonConvert.DeserializeObject<T>(responseJson);
                            }

                            return result;
                        }
                        catch (Exception ex)
                        {
                            return default(T);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseJson))
                            return JsonConvert.DeserializeObject<T>(responseJson);
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseJson))
                            return JsonConvert.DeserializeObject<T>(responseJson);
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                return null;
            }
        }

        public static async Task<T> PostOrPut(string url, Stream postStream, string name, string filePath, HttpMethod method, bool requiresToken, bool encode = true, bool handleDoubleDeserialize = false)
        {
            try
            {
                using (var client = new HttpClient(new NativeMessageHandler() { Timeout = new TimeSpan(0, 2, 0), DisableCaching = true }))
                {
                    //var cancellationTokenSrc = new CancellationTokenSource(15000);
                    //var cancellationToken = cancellationTokenSrc.Token;

                    if (!url.StartsWith("http"))
                    {
                        client.BaseAddress = new Uri(Settings.ServiceBaseURL);
                        url = $"{client.BaseAddress}{url}";
                    }

                    var postContent = new MultipartFormDataContent();//("NKdKd9Yk");

                    //postContent.Headers.ContentType.MediaType = "multipart/form-data";

                    postContent.Add(new StreamContent(postStream), $"\"{name}\"", $"\"{name}\"");

                    if (requiresToken)
                    {
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", Settings.ServiceToken);
                        client.DefaultRequestHeaders.Add("authorization", Settings.ServiceToken);
                    }

                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));



                    var response = new HttpResponseMessage();

                    if (method == HttpMethod.Post)
                    {
                        response = await client.PostAsync(url, postContent);
                    }
                    else
                    {
                        response = await client.PutAsync(url, postContent);
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && !url.Contains(AppConst.UserLoginUrl))
                    {
                        var _viewModel = App.Container.Resolve<DrawerPageViewModel>();
                        if (_viewModel != null)
                        {
                            _viewModel.Logout();
                        }

                    }
                    var data = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"POST API StatusCode : {response.StatusCode}");
                        System.Diagnostics.Debug.WriteLine("------------------------\n");

                        var responseJson = string.Empty;

                        try
                        {
                            responseJson = await response.Content.ReadAsStringAsync();

                            if (string.IsNullOrEmpty(responseJson))//(responseJson.IsEmpty())
                            {
                                return default(T);
                            }
                            ////////
                            /*
                            if ((url.Contains(AppConst.LoginApiUrl) || url.Contains(AppConst.PasswordRecoveryApiUrl)) && response.Headers != null && response.Headers.Contains("Token"))
                            {
                                string token = response.Headers.GetValues("Token").FirstOrDefault();

                                Settings.ServiceToken = token;
                            }
                            */
                            ////////
                            T result;
                            if (handleDoubleDeserialize)
                            {
                                result = JsonConvert.DeserializeObject<T>(JsonConvert.DeserializeObject<string>(responseJson));
                            }
                            else
                            {
                                result = JsonConvert.DeserializeObject<T>(responseJson);
                            }

                            return result;
                        }
                        catch (Exception ex)
                        {
                            return default(T);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseJson))
                            return JsonConvert.DeserializeObject<T>(responseJson);
                    }
                    else if (response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseJson))
                            return JsonConvert.DeserializeObject<T>(responseJson);
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                return null;
            }
        }

        public static async Task<T> Put(string url, object postData, bool requiresToken = true)
        {
            var postString = JsonConvert.SerializeObject(postData, Formatting.Indented);
            return await PostOrPut(url, postString, HttpMethod.Put, requiresToken);
        }
    }

}