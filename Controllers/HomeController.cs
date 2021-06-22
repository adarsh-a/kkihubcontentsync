using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KKIHub.ContentSync.Web.Models;
using KKIHub.ContentSync.Web.Helper;

namespace KKIHub.ContentSync.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            JsonCreator.CheckFiles();
            var syncModel = new SyncModel();
            var hubMap = Constants.Constants.HubNameToId;
            foreach (var hub in hubMap)
            {
                syncModel.HubInfo.Add(new Models.HubInfo { HubName = hub.Key, HubId = hub.Value });
            }
            return View(syncModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
