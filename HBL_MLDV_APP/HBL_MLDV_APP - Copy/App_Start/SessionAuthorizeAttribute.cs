
using HBL_MLDV_APP.Models.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace HBL_MLDV_APP.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,  Inherited = true, AllowMultiple = false)]
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override  bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["App"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    if (ApplicationSession.Session != null)// ApplicationSession.Session.UserAccountDetailObj.Count > 0)
            //    {
            //        if (ApplicationSession.Session.UserAccountDetailObj.Count > 0)
            //        {
            //            filterContext.Result = new RedirectResult(System.Web.HttpContext.Current.Request.UrlReferrer.OriginalString.ToString());
            //        }
            //        else
            //        {
            //            //LOCAL
            //            filterContext.HttpContext.Response.StatusCode = 403;
            //            filterContext.Result = new JsonResult { Data = "/Login", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //        }
            //    }
            //    else
            //    {
            //        //LOCAL
            //        filterContext.HttpContext.Response.StatusCode = 403;
            //        filterContext.Result = new JsonResult { Data = "/Login", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //    }
            //}
            //else
            //{
            //    string logon_url = WebConfigurationManager.AppSettings["logon_url"].ToString();

            //    if (ApplicationSession.Session != null)// ApplicationSession.Session.UserAccountDetailObj.Count > 0)
            //    {
            //        if (ApplicationSession.Session.UserAccountDetailObj.Count > 0)
            //        {
            //            filterContext.Result = new RedirectResult(System.Web.HttpContext.Current.Request.UrlReferrer.OriginalString.ToString());
            //        }
            //        else
            //        {
            //            //LOCAL
            //            filterContext.Result = new RedirectResult(logon_url);
            //            //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Url.Host.ToString()+"/Login");
            //        }
            //    }
            //    else
            //    {
            //        //LOCAL
            //        filterContext.Result = new RedirectResult(logon_url);
            //        //filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.Url.Host.ToString() + "/Login");
            //    }
            //}

            string logon_url = WebConfigurationManager.AppSettings["logon_url"].ToString();

            if (ApplicationSession.Session != null)// ApplicationSession.Session.UserAccountDetailObj.Count > 0)
            {
                if (ApplicationSession.Session.UserAccountDetailObj.Count > 0)
                {
                    filterContext.Result = new RedirectResult(System.Web.HttpContext.Current.Request.UrlReferrer.OriginalString.ToString());
                }
                else
                {
                    //LOCAL
                    filterContext.Result = new RedirectResult(logon_url);
                }
            }
            else
            {
                //LOCAL
                filterContext.Result = new RedirectResult(logon_url);
            }
        }
    }
}