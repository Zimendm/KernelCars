﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using KernelCars.Data;


namespace KernelCars.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private DataContext _context;
        public NavigationMenuViewComponent(DataContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke() 
        {
            //ViewBag.SelectedStatus = RouteData?.Values["CarStatus"];

            //todo заменить на Route Value
            ViewBag.SelectedStatus = (HttpContext.Request.Query["status"]).ToString();


            IEnumerable<string> statuses = from s in _context.Statuses
                                           select s.State;
            return View(statuses);
        }
    }
}
