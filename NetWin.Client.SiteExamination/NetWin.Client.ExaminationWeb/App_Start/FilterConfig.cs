using System.Web;
using System.Web.Mvc;

namespace NetWin.Client.ExaminationWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}