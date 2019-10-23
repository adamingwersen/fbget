using System;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

using Services.Proxy;

namespace Client
{
    /// <summary>
    /// Interface Framework for FacebookClient
    /// </summary>
    public interface IFacebookClient
    {
        Task<T> GetAsync<T>(string accessToken, string endpoint, string pageview, string args = null);
    }

    /// <summary>
    /// FacebookClient Class
    /// </summary>
    public class FacebookClient : IFacebookClient
    {
        private readonly HttpClient _httpClient;

        ProxyInformation proxyInfo  = new ProxyInformation();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Client.FacebookClient"/> class.
		/// </summary>
		public FacebookClient()
		{
			HttpClientHandler _httpClientHandler = new HttpClientHandler()
			{
				PreAuthenticate = true,
				UseProxy = false
			};

			_httpClient = new HttpClient(_httpClientHandler)
			{
				BaseAddress = new Uri("https://graph.facebook.com/v2.9/")
			};

			_httpClient.DefaultRequestHeaders
					   .Accept
					   .Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Client.FacebookClient"/> class.
        /// </summary>
        public FacebookClient(Tuple<string,string> proxyCredentials)
        {
            WebProxy proxy = new WebProxy("proxy-tegl", 8080);
            proxy.Credentials = new NetworkCredential(proxyCredentials.Item1, proxyCredentials.Item2);

            HttpClientHandler _httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                PreAuthenticate = true,
                UseProxy = true
            };

            _httpClient = new HttpClient(_httpClientHandler)
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.9/")
            };

            _httpClient.DefaultRequestHeaders
                       .Accept
                       .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Executes FacebookClient and handles response
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="accessToken">Access token.</param>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="pageview">Pageview.</param>
        /// <param name="args">Arguments.</param>
        /// <typeparam name="T"> A Data object class</typeparam>
        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string pageview = null, string args = null)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            if (endpoint != "posts"){
                response = await _httpClient.GetAsync($"{endpoint}/{pageview}?fields={args}&access_token={accessToken}");
            }
            else
            {
                response = await _httpClient.GetAsync($"{endpoint}?fields={args}&access_token={accessToken}");
            }


            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
