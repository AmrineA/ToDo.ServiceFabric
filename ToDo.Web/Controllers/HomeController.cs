using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToDo.Web.Logic;
using ToDo.Web.Models;

namespace ToDo.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View(await ServiceLogic.GetAsync<IEnumerable<ToDoItem>>(ConfigurationManager.AppSettings["ServiceUrl"]));
        }
    }
}