﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using KernelCars.Data;

namespace KernelCars.Controllers
{
    public class LocationsController : Controller
    {
        private DataContext _context;
        public LocationsController(DataContext ctx)
        {
            _context = ctx;
        }
        // GET: LocationsController
        public ActionResult Index()
        {
            var locations = _context.Locations.ToList();
            return View(locations);
        }

        // GET: LocationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LocationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocationsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
