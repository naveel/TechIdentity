using HBL_MLDV_APP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP.App_Start.CustomAttributes
{
    public class HandleExceptionLogAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// The method called when an exception happens
        /// </summary>
        /// <param name="filterContext">The exception context</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null)
            {
                if (!filterContext.IsChildAction && (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled))
                {
                    // Log exception to file
                    UniversalRepository universalRepository= new UniversalRepository();
                    universalRepository.WriteException(filterContext.Exception.ToString(), "Unhandled Exception!");

                    string controllerName = (string)filterContext.RouteData.Values["controller"];
                    string actionName = (string)filterContext.RouteData.Values["action"];
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                    // Set the error view to be shown
                    ViewResult result = new ViewResult
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };

                    result.ViewData["Description"] = filterContext.Controller.ViewBag.Description;
                    filterContext.Result = result;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
        }
    }
}