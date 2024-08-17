using System.Net;

namespace urlfuzz.src.Utils
{
    public class Http
    {
        private readonly HttpClient _client;
        private readonly CookieContainer _cookieContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Http"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor creates a new instance of the <see cref="HttpClient"/> class and assigns it to the
        /// <see cref="_client"/> field.
        /// </remarks>
        public Http()
        {
            _cookieContainer = new CookieContainer();
            _client = new HttpClient(new HttpClientHandler { CookieContainer = _cookieContainer });
        }

        /// <summary>
        /// Adds a cookie to the cookie container with the specified name, value, domain, and path.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="domain">The domain of the cookie.</param>
        /// <param name="path">The path of the cookie.</param>

        public void AddCookie(string name, string value, string domain, string path)
        {
            _cookieContainer.Add(new Cookie(name, value, path, domain));
        }

        /// <summary>
        /// Adds a header to the request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddHeader(string name, string value)
        {
            _client.DefaultRequestHeaders.Add(name, value);
        }

        /// <summary>
        /// Sets the User-Agent header.
        /// </summary>
        /// <param name="userAgent"></param>
        public void SetUserAgent(string userAgent) => AddHeader("User-Agent", userAgent);

        /// <summary>
        /// Sets the request timeout.
        /// </summary>
        /// <param name="timeout"></param>
        public void SetTimeout(int timeout) => _client.Timeout = TimeSpan.FromSeconds(timeout);

        /// <summary>
        /// Sends a PUT request to the specified URL with the specified data.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Put(string url, Dictionary<string, string> data)
        {
            return await _client.PutAsync(url, new FormUrlEncodedContent(data));
        }

        /// <summary>
        /// Sends a POST request to the specified URL with the specified data.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(string url, Dictionary<string, string> data)
        {
            return await _client.PostAsync(url, new FormUrlEncodedContent(data));
        }

        /// <summary>
        /// Sends a GET request to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _client.GetAsync(url);
        }

        /// <summary>
        /// Sends a DELETE request to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await _client.DeleteAsync(url);
        }

        /// <summary>
        /// Sends a HEAD request to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Head(string url)
        {
            return await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
        }

        /// <summary>
        /// Sends a PATCH request to the specified URL with the specified data.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Patch(string url, Dictionary<string, string> data)
        {
            return await _client.PatchAsync(url, new FormUrlEncodedContent(data));
        }

        /// <summary>
        /// Sends a OPTIONS request to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Options(string url)
        {
            return await _client.SendAsync(new HttpRequestMessage(HttpMethod.Options, url));
        }

        /// <summary>
        /// Sends a TRACE request to the specified URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Trace(string url)
        {
            return await _client.SendAsync(new HttpRequestMessage(HttpMethod.Trace, url));
        }

        /// <summary>
        /// Reads the response from the HTTP response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<string> ReadResponseString(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }
    }
}
