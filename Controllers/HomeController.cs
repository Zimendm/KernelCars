using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KernelCars.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {

            //=> "Hello From Kernel Cars";
            return View();
        }
    }
}