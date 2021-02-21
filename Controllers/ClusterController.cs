using KernelCars.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clusters.ToListAsync());
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
        public async Task<IActionResult> Create(
            [Bind("ClusterName")] Cluster cluster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cluster);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(cluster);
        }

        // GET: ClusterController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cluster = await _context.Clusters.FindAsync(id);
            if (cluster==null)
            {
                return NotFound();
            }
            return View(cluster);
        }

        // POST: ClusterController/Edit/5
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }

            var cluterToUpdate = await _context.Clusters.FirstOrDefaultAsync(c => c.Id == id);
            if (await TryUpdateModelAsync<KernelCars.Models.Cluster>(
                cluterToUpdate,
                "",
                c=>c.ClusterName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(cluterToUpdate);
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
