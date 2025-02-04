using System.Web;
using System.Web.Mvc;

namespace WebApplicationM06_04_01ProgramarAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
