using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.common
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class HttpUtils
    {
        private static HttpUtils _instance;
        static object obj = new object();
        public static HttpUtils Ins
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        _instance = new HttpUtils();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// http请求UA
        /// </summary>
        public string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.78 Safari/537.36";
        /// <summary>
        /// 用户登录令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// //1:使用默认代理 2:使用IE代理  3:不使用代理
        /// </summary>
        string HttpProxy = "1";

        /// <summary>
        /// 设置http请求UA
        /// </summary>
        /// <param name="ua">UserAgent</param>
        /// <param name="proxy">HttpProxy 1:使用默认代理 2:使用IE代理  3:不使用代理</param>
        public void InitHttpUtils(string ua = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36", string proxy = "1")
        {
            if (!string.IsNullOrEmpty(ua))
                UserAgent = ua;
            if (!string.IsNullOrEmpty(proxy))
                HttpProxy = proxy;
        }
        /// <summary>
        /// GET请求获取信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GET(string url, string encode = "UTF-8", int timeout = 5000, List<Cookie> cookies = null)
        {
            ClassLoger.Info("HttpUtils.Get",url);
            string ret = string.Empty;
            try
            {
                HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    web.ProtocolVersion = HttpVersion.Version10;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }
                SetProxy(ref web);
                web.Method = "GET";
                web.Timeout = timeout;
                web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //web.Connection = "keep-alive";
                web.KeepAlive = true;
                web.IfModifiedSince = DateTime.Now;
                web.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");

                web.Headers.Add("X-CSRF-Token", "aDkFA0RIlVpHySSCFTkhQ62fPUzqhpFzC8nWxIUlgYOZwYtGdkFQwL6Ppii1/ngiCT9sSkSGIS2EMO4vrXUmqA==");


                web.Date = DateTime.Now;
                if (!string.IsNullOrEmpty(UserAgent))
                    web.UserAgent = UserAgent;
                if (cookies != null && cookies.Count > 0)
                {
                    web.CookieContainer = new CookieContainer();

                    string host = new Uri(url).Host;
                    foreach (Cookie c in cookies)
                    {
                        c.Domain = host;
                        web.CookieContainer.Add(c);
                    }
                }
                HttpWebResponse res = (HttpWebResponse)web.GetResponse();
                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding(encode));
                ret = sr.ReadToEnd();
                sr.Close();
                res.Close();
                s.Close();
                //ClassLoger.Error("HttpUtils/GET",url+" "+ ret);
            }
            catch (Exception ex)
            {
                ClassLoger.Error("HttpUtils/GET", url + "," + ex.Message);
                throw ex;
            }
            return ret;
        }

        public string GETbyJson(string url, string encode = "UTF-8", int timeout = 5000, List<Cookie> cookies = null)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    web.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref web);
                web.Method = "GET";
                web.Timeout = timeout;
                //web.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                web.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
                //web.Headers["Connection"] = "keep-alive";
                //web.Headers.Add("Connection", "keep-alive");
                web.ContentType = "application/json";
                if (!string.IsNullOrEmpty(UserAgent))
                    web.UserAgent = UserAgent;
                if (!string.IsNullOrEmpty(Token))
                {
                    web.Headers.Add("Authorization", Token);
                }
                if (cookies != null && cookies.Count > 0)
                {
                    web.CookieContainer = new CookieContainer();

                    string host = new Uri(url).Host;
                    foreach (Cookie c in cookies)
                    {
                        c.Domain = host;
                        web.CookieContainer.Add(c);
                    }
                }
                HttpWebResponse res = (HttpWebResponse)web.GetResponse();
                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding(encode));
                ret = sr.ReadToEnd();
                sr.Close();
                res.Close();
                s.Close();
                //ClassLoger.Error("HttpUtils/GET",url+" "+ ret);
            }
            catch (Exception ex)
            {
                ClassLoger.Error("HttpUtils/GET", url + "," + ex.Message);
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// POST请求获取信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public string POST(string url, string paramData,bool isJson=true, int timeout = 5000, List<Cookie> cookies = null)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    webReq.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref webReq);
                webReq.Method = "POST";
                
                if(isJson)
                    webReq.ContentType = "application/json";
                else
                    webReq.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                webReq.ServicePoint.Expect100Continue = false;
                //webReq.ContentType = "text/html;charset=utf-8";
                webReq.Timeout = timeout;
                webReq.ContentLength = byteArray.Length;
                if (!string.IsNullOrEmpty(Token))
                {
                    webReq.Headers.Add("Authorization", Token);
                }
                if (!string.IsNullOrEmpty(UserAgent))
                    webReq.UserAgent = UserAgent;
                if (cookies != null && cookies.Count > 0)
                {
                    webReq.CookieContainer = new CookieContainer();

                    string host = new Uri(url).Host;
                    foreach (Cookie c in cookies)
                    {
                        c.Domain = host;
                        webReq.CookieContainer.Add(c);
                    }
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("HttpUtils/POST", url + "," + ex.Message);
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// POST请求获取信息(上传)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public string POST(string url, string paramData, byte[] data, int timeout = 5000, List<Cookie> cookies = null)
        {
            return POST(string.Format("{0}?{1}", url, paramData), data, timeout, cookies);
        }
        /// <summary>
        /// POST请求获取信息(上传)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramData"></param>
        /// <returns></returns>
        public string POST(string url, byte[] data, int timeout = 5000, List<Cookie> cookies = null)
        {
            string ret = string.Empty;
            try
            {
                string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    webReq.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref webReq);
                webReq.Method = "POST";
                webReq.ContentType = "multipart/form-data; boundary=" + boundary;
                webReq.ServicePoint.Expect100Continue = false;
                //webReq.ContentType = "text/html;charset=utf-8";
                webReq.Timeout = timeout;
                webReq.ContentLength = data.Length;
                if (!string.IsNullOrEmpty(Token))
                {
                    webReq.Headers.Add("Authorization", Token);
                }
                if (!string.IsNullOrEmpty(UserAgent))
                    webReq.UserAgent = UserAgent;
                if (cookies != null && cookies.Count > 0)
                {
                    webReq.CookieContainer = new CookieContainer();

                    string host = new Uri(url).Host;
                    foreach (Cookie c in cookies)
                    {
                        c.Domain = host;
                        webReq.CookieContainer.Add(c);
                    }
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(data, 0, data.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("HttpUtils/POST", url + "," + ex.Message);
                throw ex;
            }
            return ret;
        }
        /// <summary>
        /// 模拟httPost提交Form表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public  string CreateAutoSubmitForm(string url, Dictionary<string, string> data, Encoding encoder)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"text/html; charset={0}\" />", encoder.BodyName);
            html.AppendLine("</head>");
            html.AppendLine("<body onload=\"OnLoadSubmit();\">");
            html.AppendFormat("<form id=\"pay_form\" action=\"{0}\" method=\"post\">", url);
            foreach (KeyValuePair<string, string> kvp in data)
            {
                html.AppendFormat("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            html.AppendLine("</form>");
            html.AppendLine("<script type=\"text/javascript\">");
            html.AppendLine("<!--");
            html.AppendLine("function OnLoadSubmit()");
            html.AppendLine("{");
            html.AppendLine("document.getElementById(\"pay_form\").submit();");
            html.AppendLine("}");
            html.AppendLine("//-->");
            html.AppendLine("</script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }
        public string POST(string url, string param, Encoding w_encoding, Encoding r_encoding, int timeout = 5000, string contenttype = "application/x-www-form-urlencoded")
        {
            string ret = string.Empty;
            try
            {
                w_encoding = w_encoding == null ? System.Text.Encoding.Default : w_encoding;
                r_encoding = r_encoding == null ? System.Text.Encoding.Default : r_encoding;
                byte[] byteArray = w_encoding.GetBytes(param); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = contenttype;
                webReq.Timeout = timeout;
                webReq.ContentLength = byteArray.Length;
                webReq.UserAgent = UserAgent;
                webReq.Proxy = null;
                if (!string.IsNullOrEmpty(Token))
                {
                    webReq.Headers.Add("Authorization", Token);
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), r_encoding);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="savepath">保存路径</param>
        public bool DownFile(string url, string savepath)
        {
            if (url.IsNull() || savepath.IsNull())
                return false;
            try
            {
                string dir_path = savepath.Replace(Path.GetFileName(savepath), "");
                if (!Directory.Exists(dir_path))
                    Directory.CreateDirectory(dir_path);
                HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    web.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref web);
                web.Method = "GET";
                if (!string.IsNullOrEmpty(UserAgent))
                    web.UserAgent = UserAgent;
                HttpWebResponse res = (HttpWebResponse)web.GetResponse();
                Stream s = res.GetResponseStream();
                byte[] data = new byte[1024 * 100];
                using (FileStream fs = File.Open(savepath, FileMode.Create))
                {
                    int count = 0;
                    while ((count = s.Read(data, 0, data.Length)) > 0)
                        fs.Write(data, 0, count);
                    fs.Close();
                    fs.Dispose();
                }
                s.Close();
                s.Dispose();
                res.Close();
                if (File.Exists(savepath))
                    return true;
                else
                    return false;
                //ClassLoger.Error("HttpUtils/GET",url+" "+ ret);
            }
            catch (Exception ex)
            {
                if (File.Exists(savepath))
                    File.Delete(savepath);
                ClassLoger.Error("HttpUtils/GET", url + "," + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// 下载文本
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string DownString(string url)
        {
            try
            {
                HttpWebRequest web = (HttpWebRequest)HttpWebRequest.Create(url);
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    web.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref web);
                web.Method = "GET";
                if (!string.IsNullOrEmpty(UserAgent))
                    web.UserAgent = UserAgent;
                HttpWebResponse res = (HttpWebResponse)web.GetResponse();
                Stream s = res.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                byte[] data = new byte[1024 * 100];
                int count = 0;
                while ((count = s.Read(data, 0, data.Length)) > 0)
                {
                    ms.Write(data, 0, count);
                }
                string str = System.Text.Encoding.Default.GetString(ms.ToArray());
                s.Close();
                s.Dispose();
                res.Close();
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断文件是否有更新
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localmodifieddate"></param>
        /// <returns></returns>
        public bool FileIsModified(string url, DateTime localmodifieddate, int timeout = 5000)
        {
            bool isModified = true;
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                if (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate); //rcvc;
                    webReq.ProtocolVersion = HttpVersion.Version10;
                }
                SetProxy(ref webReq);
                webReq.Method = "GET";
                webReq.IfModifiedSince = localmodifieddate;

                webReq.Timeout = timeout;
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                response.Close();
            }
            catch (WebException wex)
            {
                HttpWebResponse response = (HttpWebResponse)wex.Response;
                if (response.StatusCode == HttpStatusCode.NotModified)
                    isModified = false;
                response.Close();
            }
            catch (Exception ex)
            {
                ClassLoger.Error("HttpUtils/FileIsModified", url, ex);
                throw ex;
            }
            return isModified;
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public string UrlEncode(string v)
        {
            return System.Web.HttpUtility.UrlEncode(v);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="v"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string UrlEncode(string v, Encoding charset)
        {
            return System.Web.HttpUtility.UrlEncode(v, charset);
        }

        /// <summary>
        /// URL 转码
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public string UrlDecode(string v)
        {
            return System.Web.HttpUtility.UrlDecode(v);
        }

        /// <summary>
        /// URL 转码
        /// </summary>
        /// <param name="v"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public string UrlDencode(string v, Encoding charset)
        {
            return System.Web.HttpUtility.UrlDecode(v, charset);
        }


        /// <summary>
        /// 1:使用(默认) 2:使用IE代理  3：不使用代理
        /// </summary>
        /// <param name="wr"></param>
        /// <returns></returns>
        private void SetProxy(ref HttpWebRequest wr)
        {
            if (wr == null)
                return;
            switch (HttpProxy)//AppSet.TProxy
            {
                case "1":
                    break;
                case "2":
                    wr.Proxy = WebRequest.GetSystemWebProxy();
                    break;
                case "3":
                    wr.Proxy = null;
                    break;
                default:
                    goto case "1";
            }
        }

        bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
