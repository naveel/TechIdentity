using HBL_MLDV_APP.App_Start;
using HBL_MLDV_APP.Models.Security;
using HBL_MLDV_APP.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HBL_MLDV_APP.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        UniversalRepository universalRepository = new UniversalRepository();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
    }
}