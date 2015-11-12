using GeocachingExample.Data;
using GeocachingExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeocachingExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //get a list of Geocache items and pass them to the index view.
            return View(new Repository().GetGeocacheItemNamesList());
        }
    }
}
