using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NetWin.Client.ExaminationWeb.Models;

namespace NetWin.Client.ExaminationWeb.Controllers
{
    public class ExaminationController : Controller
    {
        //
        // GET: /Examination/

        public ActionResult Index(bool isError = false, string message = "", string url = "")
        {
            ViewData["url"] = url;
            ViewData["isError"] = isError;
            ViewData["message"] = message;
            return View();
        }

        /// <summary>
        /// 开始体检
        /// </summary>
        /// <param name="examinationUrl"></param>
        /// <returns></returns>
        public ActionResult StartPage(string examinationUrl, int siteId=0, bool isHostory=false)
        {
            if (isHostory)
            {
                ViewData["isHostory"] = isHostory;
                ViewData["siteId"] = siteId;
                return View();
            }
         
            if (string.IsNullOrWhiteSpace(examinationUrl))
            {
                ViewData["isError"] = true;
                ViewData["message"] = "输入的网址不能为空!";
                return View("Index");
            }
            examinationUrl = examinationUrl.Trim();
            if (!examinationUrl.Contains("http://"))
            {
                examinationUrl = "http://" + examinationUrl;
            }

            if (!CheckURLByString(examinationUrl))
            {
                ViewData["url"] = examinationUrl;
                ViewData["isError"] = true;
                ViewData["message"] = "输入的网址不合法!";
                return View("Index");
            }

            ViewData["url"] = GetDomainName(examinationUrl);
            ViewData["isHostory"] = isHostory;
            ViewData["siteId"] = siteId;
            return View();
        }

        /// <summary>
        /// 匹配URL是否合法
        /// </summary>
        /// <param name="source">待匹配字符串</param>
        /// <returns>匹配结果true是URL反之不是URL</returns>
        private bool CheckURLByString(string source)
        {
            Regex rg = new Regex(@"^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);
            return rg.IsMatch(source);
        }

        /// <summary>
        /// 获取网站域名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private  string GetDomainName(string url)
        {
            try
            {
                const string pattern = @"https?://(.*?)($|/)";
                string host = Regex.Match(url, pattern, RegexOptions.IgnoreCase).Value;
                if (!string.IsNullOrWhiteSpace(host))
                    host = host.TrimEnd('/');
                return host;
            }
            catch
            {
                // ignored
            }
            return string.Empty;
        }
    }
}
