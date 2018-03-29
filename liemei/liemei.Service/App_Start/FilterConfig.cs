using liemei.Service.Filters;
using System.Web;
using System.Web.Mvc;

namespace liemei.Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DExceptionFilterAttribute());
            filters.Add(new CustomAuthenticationFilter());
        }
    }
}
