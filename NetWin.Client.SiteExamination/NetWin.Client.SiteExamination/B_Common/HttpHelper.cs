using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

using System.Net;
using System.Text;

namespace NetWin.Client.SiteExamination.B_Common
{
    public class HttpHelper
    {
        /// <summary>
        /// 请求返回网页内容
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="timeOut">设置超时时间</param>
        /// <returns></returns>
        public static ResponseMessage RequestSite(string url, int timeOut = 10)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            ServicePointManager.DefaultConnectionLimit = 50;
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                string htmlContent = string.Empty;
                webRequest.Timeout = timeOut * 1000; //超时
                webRequest.Method = "GET"; //请求方法
                webRequest.KeepAlive = false;
                webRequest.Accept = "text/html"; //接受的内容
                webRequest.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.16 Safari/537.36";
                ; //用户代理
                //  webRequest.Credentials = CredentialCache.DefaultCredentials;
                webResponse = (HttpWebResponse)webRequest.GetResponse();

                string acceptEncoding = webResponse.Headers["Content-Encoding"] ?? ""; //获得压缩格式
                byte[] bytes;
                if (acceptEncoding.Contains("gzip"))
                {
                    using (GZipStream gzip = new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress)
                        )
                    {
                        bytes = IoHelper.StreamToBytes(gzip);
                    }
                }
                else
                {
                    bytes = IoHelper.StreamToBytes(webResponse.GetResponseStream());
                }
                var stream = new StreamReader(IoHelper.BytesToStream(bytes), Encoding.GetEncoding("utf-8"));
                htmlContent = stream.ReadToEnd();
                var encodingString = RegexHelper.GetEncoding(htmlContent);

                //判断不是utf8不转码,则进行在编码
                if (encodingString.ToLower() != "utf-8")
                {
                    htmlContent =
                        new StreamReader(IoHelper.BytesToStream(bytes), Encoding.GetEncoding(encodingString)).ReadToEnd();
                }
                responseMessage.InnerHtml = htmlContent;
                responseMessage.StatusCode = Convert.ToInt32(webResponse.StatusCode);
                responseMessage.LastModified = webResponse.LastModified;
                responseMessage.ResponseUrls = webResponse.ResponseUri.ToString();
                responseMessage.Size = bytes.Length / 1024;
                webResponse.Close();
                webRequest.GetResponse().Close();
            }
            catch (WebException exception)
            {
                HttpWebResponse response = (HttpWebResponse)exception.Response;
                if (response != null)
                {
                    responseMessage.StatusCode = Convert.ToInt32(response.StatusCode);
                    responseMessage.LastModified = response.LastModified;
                    responseMessage.ResponseUrls = response.ResponseUri.ToString();
                    response.Close();
                }

            }
            finally
            {
                if (webResponse != null)
                    webResponse.Close();

                if (webRequest != null) 
                    webRequest.Abort();
            }
            return responseMessage;
        }

        /// <summary>
        /// 返回请求链接的状态码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static int GetStatusCode(string url)
        {
            int code = 0;
            try
            {
                WebRequest request = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                code = Convert.ToInt32(response.StatusCode);
            }
            catch (WebException exception)
            {
                HttpWebResponse response = (HttpWebResponse)exception.Response;
                code = Convert.ToInt32(response.StatusCode);
            }
            return code;
        }
    }


    public class ResponseMessage
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { set; get; }

        public DateTime LastModified { set; get; }

        public string InnerHtml { set; get; }

        public string ResponseUrls { set; get; }

        /// <summary>
        /// 大小,单位kb
        /// </summary>
        public long Size { set; get; }
    }
}
