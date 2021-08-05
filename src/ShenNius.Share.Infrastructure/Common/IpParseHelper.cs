using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace ShenNius.Share.Infrastructure.Common
{
    public class IpParseHelper
    {
        public static string GetAddressByIP(string IP)
        {
            try
            {
                if (string.IsNullOrEmpty(IP))
                {
                    return IP;
                }
                string url = "http://whois.pconline.com.cn/ipJson.jsp?callback=testJson&ip=" + IP;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "text/html;chartset=UTF-8";
                request.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 48.0) Gecko / 20100101 Firefox / 48.0"; //火狐用户代理
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream streamResponse = response.GetResponseStream())
                    {
                        using (StreamReader streanReader = new StreamReader(streamResponse, Encoding.GetEncoding("gb2312")))
                        {
                            string retString = streanReader.ReadToEnd();

                            string t = retString.Substring(retString.IndexOf("{\""), retString.IndexOf(");}") - retString.IndexOf("{\""));
                            Ipinfos m = (Ipinfos)JsonConvert.DeserializeObject(t, typeof(Ipinfos));

                            string IPProvince = m?.Pro == "" ? "其它地区" : m.Pro;
                            string IPCity = m?.City;
                            return $"{IPProvince}-{IPCity}";
                        }
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public class Ipinfos
        {
            public string Pro { get; set; }
            public string City { get; set; }
        }
    }
}
