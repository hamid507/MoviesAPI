using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utility
{
    public static class ApiUtils
    {
        private static HttpClient _httpClient = null;

        private static HttpClient httpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                    _httpClient = new HttpClient(clientHandler)
                    {
                        Timeout = TimeSpan.FromSeconds(60)
                    };

                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                }

                return _httpClient;
            }
        }
        private static HttpRequestMessage CreateHttpRequestMessage(string apiUrl, HttpMethod httpMethod, string data = null)
        {
            HttpRequestMessage request;

            if (data == null)
            {
                request = new HttpRequestMessage
                {
                    Method = httpMethod,
                    RequestUri = new Uri(apiUrl),
                    Version = new Version(1, 0)
                };
            }
            else
            {
                request = new HttpRequestMessage
                {
                    Method = httpMethod,
                    RequestUri = new Uri(apiUrl),
                    Content = new StringContent(data, Encoding.UTF8, "application/json"),
                    Version = new Version(1, 0)
                };
            }

            return request;
        }

        private static async Task<string> ApiRequestAsync(HttpRequestMessage httpRequestMsg)
        {
            string result = null;

            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMsg).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //TO DO: Log
                }

            }
            catch (Exception ex)
            {
                //TO DO: Log
            }

            return result;
        }

        public static async Task<T> SendGetRequestAsync<T>(string url, object bodyContent, bool deserialize = true)
        {
            T result = default;

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var tempJsonBodyContent = JsonConvert.SerializeObject(bodyContent);
                    var httpReqMsg = CreateHttpRequestMessage(url, HttpMethod.Get, tempJsonBodyContent);
                    string responseString = await ApiRequestAsync(httpReqMsg);

                    if (deserialize && !string.IsNullOrEmpty(responseString))
                    {
                        result = JsonConvert.DeserializeObject<T>(responseString);
                    }

                }
            }
            catch (Exception ex)
            {
                //TO DO: Log
            }

            return result;
        }

        public static async Task<T> SendPostRequestAsync<T>(string url, object bodyContent, bool deserialize = true)
        {
            T result = default;

            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var tempJsonBodyContent = JsonConvert.SerializeObject(bodyContent);
                    var httpReqMsg = CreateHttpRequestMessage(url, HttpMethod.Post, tempJsonBodyContent);

                    string responseString = await ApiRequestAsync(httpReqMsg);

                    if (deserialize && !string.IsNullOrEmpty(responseString))
                    {
                        result = JsonConvert.DeserializeObject<T>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                //TO DO: Log
            }

            return result;
        }

        public static byte[] DownloadImageFromUrl(string url)
        {
            using(WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }
    }
}
