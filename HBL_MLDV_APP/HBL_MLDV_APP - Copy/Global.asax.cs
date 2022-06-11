using HBL_MLDV_APP.App_Start.CustomAttributes;
using HBL_MLDV_APP.Models;
using HBL_MLDV_APP.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HBL_MLDV_APP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [HandleExceptionLog]
        protected void Application_Start()
        {
            Enums.ApiUri = WebConfigurationManager.AppSettings["api"].ToString();
            //Enums.ScreeningApiUri = WebConfigurationManager.AppSettings["screeningapi"].ToString();
            Enums.sess_exp_val = WebConfigurationManager.AppSettings["sess_exp_val"].ToString();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}
