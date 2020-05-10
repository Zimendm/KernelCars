using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KernelCars.Data;
using KernelCars.Models;
using KernelCars.Models.ViewModels;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net;


namespace KernelCars.Controllers
{
    public class CarServicesController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CarServicesController(DataContext context,
              IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: CarServices
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.CarServices;//.Include(c => c.Car);
            return View(await dataContext.ToListAsync());
            //return View();

        }

        // GET: CarServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = await _context.CarServices
                .Include(c => c.Car)
                .Include(cs => cs.WorkAssigments)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carService == null)
            {
                return NotFound();
            }

            //string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, @"AccountingDocuments");
            //string filePath = Path.Combine(uploadFolder, carService.DocumentPath);

            //ViewData["Scan"] = "~/AccountingDocuments/"+ carService.DocumentPath;

            PopulateAssignedWorks(carService);
            return View(carService);
        }

        [HttpGet]
        public async Task<IActionResult> GetPDF(string fileName)
        {
            string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, @"AccountingDocuments");
            string filePath = Path.Combine(uploadFolder, fileName);
            //string filePath = Path.Combine(uploadFolder, "Test.pdf");

            //Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);
            //return File(filePath, "application/pdf","testFile.pdf");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            //GetContentType(filePath)

            Response.Headers.Add("Content-Disposition", $"inline; filename=\"{fileName}\"");
            return File(memory, "application/pdf", fileName);



        }

        [HttpGet]
        public FileStreamResult GetPDFtoPreview(string fileName)
        {
            FileStream fs = new FileStream(Path.Combine(Path.Combine(hostingEnvironment.WebRootPath, @"AccountingDocuments"), fileName), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public IActionResult CarSeviceList(int carId)
        {

            var service = _context.CarServices.Include(s => s.Car).Where(s => s.CarId == carId).ToList();
            Car car = _context.Cars.Where(c => c.Id == carId).FirstOrDefault();
            ViewData["RegistrationNumber"] = car.RegistrationNumber.ToString();
            ViewData["CarId"] = (long) car.Id;
           
            return View(service);
        }
        // GET: CarServices/Create
        public IActionResult Create(int carId)
        {
            //rService carservice = _context.CarServices.Include(c => c.WorkAssigments).ThenInclude(c => c.TypeOfService);


            //Todo Вынести в отдельную процедуру

            var allServices = _context.TypeOfServices;


            //var workServices = new HashSet<int>( carservice..w instructor.CourseAssignments.Select(c => c.CourseID));
            
            var viewModel = new List<AssignedTypeOfService>();
            
            foreach (var service in allServices)
            {
                viewModel.Add(new AssignedTypeOfService
                {
                    TypeOfServiceID = service.ID,
                    Service=service.Service
                });
            }
            ViewData["Services"] = viewModel;
            ViewData["StationId"] = new SelectList(_context.ServiceStations.OrderBy(s=>s.Name), "ID", "Name");
            //var car = (from c in _context.Cars
            //          where c.Id == carId
            //          select c).FirstOrDefault();
            var car = _context.Cars.Include(c => c.CarModel.Manufacturer).Where(c => c.Id == carId).FirstOrDefault();

            CarService s = new CarService()
            {
                OpenDate = DateTime.Now,
                
                Car=car
            };
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id");
            return View(s);
        }

        // POST: CarServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OpenDate,CompleteDate,ServiceDescription,CarId,ServiceStationID")] CarService carService, string[] selectedWorks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carService);
                await _context.SaveChangesAsync();

                var serviceToUpdate = carService; // _context.CarServices
                //.Include(s => s.WorkAssigments)
                //.Where(s => s.ID == id)
                //.Single();
                serviceToUpdate.WorkAssigments = new List<WorkAssigment>();

                UpdateCarServiceWorks(selectedWorks, serviceToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                UpdateCarServiceWorks(selectedWorks, serviceToUpdate);
                PopulateAssignedWorks(serviceToUpdate);
                return RedirectToAction(nameof(CarSeviceList),new { carId = carService.CarId });
            }
















            
                return RedirectToAction(nameof(Index));
            //}
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);


            return View(carService);
        }

        public IActionResult Close(int? id)
        {
            var carService = _context.CarServices.Include(s => s.Car).Include(cs => cs.WorkAssigments).Where(c => c.ID == id).FirstOrDefault();

            PopulateAssignedWorks(carService);
            
            CarCloseServiceViewModel csvm = new CarCloseServiceViewModel
            {
                CarServiceId = carService.ID,
                CarService = carService
            };

            return View(csvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Close([Bind("CarServiceId", "CarService", "DocumentScan")] CarCloseServiceViewModel model)
        {
            string uniqueFileName = null;
            if (ModelState.IsValid)
            {
                try
                {
                    
                    if (model.DocumentScan != null)
                    {
                        string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, @"C:\Users\dzime\Source\Repos\Zimendm\KernelCars\wwwroot\AccountingDocuments\");
                        uniqueFileName = Guid.NewGuid() + "_" + model.DocumentScan.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.DocumentScan.CopyTo(stream);
                        }

                        string z1 = (string)RouteData.Values["id"];
                    }
                }
                catch (Exception)
                {

                    //throw;
                }

                //return NotFound();
                var carServiceToClose = _context.CarServices.Where(cs => cs.ID == model.CarServiceId).FirstOrDefault();
                
                if (model.CarService.CompleteDate!=null)
                {
                    carServiceToClose.CompleteDate = model.CarService.CompleteDate;
                }
                if (model.CarService.Odometr!=0)
                {
                    carServiceToClose.Odometr = model.CarService.Odometr;
                }
                if (model.CarService.Ammount > 0)
                {
                    carServiceToClose.Ammount = model.CarService.Ammount;
                }
                if (uniqueFileName!=null)
                {
                    carServiceToClose.DocumentPath = uniqueFileName;
                }

                _context.SaveChanges();
            }
            string z = (string) RouteData.Values["id"];

            return RedirectToAction("List","Car");// toToAction(nameof())
        }

        // GET: CarServices/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = _context.CarServices.Include(cs => cs.WorkAssigments).Where(cs => cs.ID == id).Single();
            if (carService == null)
            {
                return NotFound();
            }

            PopulateAssignedWorks(carService);
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);
            
            
            return View(carService);
        }

        private void PopulateAssignedWorks(CarService carService)
        {
            var allServices = _context.TypeOfServices;
            var currentServices = new HashSet<int>(carService.WorkAssigments.Select(c => c.TypeOfServiceId));
            var viewModel = new List<AssignedTypeOfService>();
            foreach (var service in allServices)
            {
                viewModel.Add(new AssignedTypeOfService
                {
                    TypeOfServiceID = service.ID,
                    Service = service.Service,
                    IsAssigned = currentServices.Contains(service.ID)
                });
            }

            ViewBag.Works = viewModel;
            
            //throw new NotImplementedException();
        }

        // POST: CarServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedWorks)
        {
            if (id==null)
            {
                return StatusCode(500);
            }
            var serviceToUpdate = _context.CarServices
                .Include(s => s.WorkAssigments)
                .Where(s => s.ID == id)
                .Single();
            if (await TryUpdateModelAsync<CarService>(serviceToUpdate,"",s=>s.OpenDate,s=>s.Odometr))
            {
                UpdateCarServiceWorks(selectedWorks, serviceToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }


            //if (id != carService.ID)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(carService);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!CarServiceExists(carService.ID))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", carService.CarId);
            //return View(carService);

            UpdateCarServiceWorks(selectedWorks, serviceToUpdate);
            PopulateAssignedWorks(serviceToUpdate);
            return View(serviceToUpdate);
            //return NotFound();
        }

        private void UpdateCarServiceWorks(string[] selectedWorks, CarService serviceToUpdate)
        {
            if (selectedWorks == null)
            {
                serviceToUpdate.WorkAssigments = new List<WorkAssigment>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedWorks);
            var instructorCourses = new HashSet<int>
                (serviceToUpdate.WorkAssigments.Select(c => c.TypeOfService.ID));
            foreach (var course in _context.TypeOfServices)
            {
                if (selectedCoursesHS.Contains(course.ID.ToString()))
                {
                    if (!instructorCourses.Contains(course.ID))
                    {
                        serviceToUpdate.WorkAssigments.Add(new WorkAssigment { CarServiceID = serviceToUpdate.ID, TypeOfServiceId = course.ID });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(course.ID))
                    {
                        WorkAssigment courseToRemove = serviceToUpdate.WorkAssigments.FirstOrDefault(i => i.TypeOfServiceId == course.ID);
                        _context.Remove(courseToRemove);
                    }
                }
            }






















            //throw new NotImplementedException();
        }

        // GET: CarServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carService = await _context.CarServices
                .Include(c => c.Car)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carService == null)
            {
                return NotFound();
            }

            return View(carService);
        }

        // POST: CarServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carService = await _context.CarServices.FindAsync(id);
            _context.CarServices.Remove(carService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarServiceExists(int id)
        {
            return _context.CarServices.Any(e => e.ID == id);
        }
    }
}
