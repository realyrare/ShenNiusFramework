using Blog.ShenNius.Layui.Admin.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ShenNius.Layui.Admin.Common
{
    public class HttpHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly DomainConfig _domainConfig;
        public HttpHelper(IHttpClientFactory httpClientFactory, IOptionsMonitor<DomainConfig> domainConfig)
        {
            _httpClientFactory = httpClientFactory;
            _domainConfig = domainConfig.CurrentValue;
        }
        public async Task<T> GetAsync<T>(string queryString,string token=null) where T : class
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string url = _domainConfig.ApiHost + queryString;
                if (!string.IsNullOrEmpty(token))
                {
                    // 'Authorization': 'Bearer ' + token
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }               
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
               
                var json = await response.Content.ReadAsStringAsync();
                if (json == null)
                {
                    throw new ArgumentNullException("api 接口要序列化的json流为空");
                }
                T model = JsonConvert.DeserializeObject<T>(json);
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                return model ?? default;
            }
            catch (Exception ex)
            {
               // WebHelper.WriteLog($"\r\n{DateTime.Now}:{ex}");
                return default;
            }
           
        }
        public async Task<T> PostAsync<T>(string url, string postData = null, string contentType = null, Dictionary<string, string> headers = null)
        {
            url = _domainConfig.ApiHost + url;
            postData ??= "";
            var client = _httpClientFactory.CreateClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            if (headers != null)
            {
                foreach (var header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
            {
                if (contentType != null)
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                var response = await client.PostAsync(url, httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentNullException(nameof(response));
                }
                var json= await response.Content.ReadAsStringAsync();
                T model = JsonConvert.DeserializeObject<T>(json);
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                return model ?? default;
            }

        }
    }
}
