using KernelCars.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Controllers
{
    public class ClusterController : Controller
    {
        DataContext _context;

        public ClusterController(DataContext ctx)
        {
            _context = ctx;
        }
        // GET: ClusterController
        public ActionResult Index()
        {
            return View(_context.Clusters.ToList());
        }

        // GET: ClusterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClusterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClusterController/Create
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

        // GET: ClusterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClusterController/Edit/5
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

        // GET: ClusterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClusterController/Delete/5
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
