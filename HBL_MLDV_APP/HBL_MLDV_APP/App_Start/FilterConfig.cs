using HBL_MLDV_APP.App_Start.CustomAttributes;
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionLogAttribute());
        }
    }
}
