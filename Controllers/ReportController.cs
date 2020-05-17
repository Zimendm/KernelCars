﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KernelCars.Infrastructure;
using KernelCars.Pages.Cars;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;

namespace KernelCars.Controllers
{
    public class ReportController : Controller
    {
        private DataContext _context;
        public ReportController(DataContext context)
        {
            _context = context;
        }

        public IActionResult CarService(long? carId, DateTime? fromDate, DateTime? toDate)
        {
            List<DateTime> dates;
            

            var services = new Dictionary<string, int>();
            var odometr = new List<int>();
            float totalAmmount=0;
                        
            

            var serviceData = _context.CarServices.Include(s => s.WorkAssigments).ThenInclude(s=>s.TypeOfService).Where(c => c.CarId == 843);
            
            if ( fromDate != null && toDate != null)
            {
                dates = new List<DateTime>();
                dates.Add((DateTime)fromDate);
                dates.Add((DateTime)toDate);
                serviceData = from d in serviceData
                              where DateTime.Compare((DateTime) d.CompleteDate, (DateTime) dates.Min()) >= 0 && DateTime.Compare((DateTime)d.CompleteDate, ((DateTime) dates.Max()).AddDays(1)) <= 0
                              select d;
                ViewData["FromDate"] = dates.Min().ToString("yyyy-MM-dd");
                ViewData["ToDate"] = dates.Max().ToString("yyyy-MM-dd");
            }


            if (serviceData.Count() > 0)
            {
                foreach (var item in serviceData)
                {
                var serDetails = from sd in item.WorkAssigments
                                 group sd by sd.TypeOfService.Service into dsl
                                 select dsl;
                totalAmmount += item.Ammount;
                odometr.Add(item.Odometr);

                foreach (var serv in serDetails)
                {
                    if (services.ContainsKey(serv.Key))
                    {
                        services[serv.Key]++;
                    }
                    else
                    {
                        services.Add(serv.Key, 1);
                    }
                }
                }

                //services.Add("Всего", totalAmmount);


                //var serDetails = from sd in serviceData.WorkAssigments
                //                 group sd by sd.TypeOfService.Service into dsl
                //                 select new { Name = dsl.Key, Amount = dsl.Count() };



                //var line  = from l in serviceData. _context.CarServices.Include(s=>s.WorkAssigments)
                //            group l.WorkAssigm

                //            in data.GroupBy(info => info.metric)
                //            .Select(group => new {
                //                Metric = group.Key,
                //                Count = group.Count()
                //            })
                //            .OrderBy(x => x.Metric)

                if (odometr.Max() - odometr.Min() > 0)
                {   
                    ViewData["Odometr"] = "Пробег за период: "+ (odometr.Max() - odometr.Min()).ToString();
                }
            }

            if (ViewData["ToDate"] == null)
            {
                ViewData["ToDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
            
            ViewData["TotalAmmount"] = totalAmmount;
            return View(services);
        }

        public IActionResult Services()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Services (DateTime? fromDate, DateTime? toDate)
        {
            List<DateTime> dates;
            dates = new List<DateTime>();
            System.Data.DataTable dtServices=new System.Data.DataTable();
            var servicesCount = new Dictionary<string, int>();
            

            //dtServices.Columns.Add(nameof());

            var typeOfServices = _context.TypeOfServices.ToList();

            dtServices.Columns.Add("Гос.номер");
            dtServices.Columns.Add("Модель");
            foreach (var item in typeOfServices)
            {
                dtServices.Columns.Add(item.Service);
            }
            dtServices.Columns.Add("Итого");





            fromDate = DateTime.Now.AddMonths(-1);
            toDate = DateTime.Now;

            if (fromDate != null && toDate != null)
            {
                dates.Add((DateTime)fromDate);
                dates.Add((DateTime)toDate);

                var carList = _context.CarServices.ToList();// .GroupBy(
         
                var cars = from car in carList
                           group car by car.CarId into ids
                           select ids;

                               //car => car.CarId,
                               //(carId) => new
                               //{
                               //    Key = carId
                               //});

                               //s => s.CarId);// _1_Pages_Cars_Index from car in _context.CarServices
                               // group car by car.CarId into ids
                               // select ids;

                foreach (var id in cars)
                {
                    var services = _context.CarServices.Include(s => s.WorkAssigments).ThenInclude(w => w.TypeOfService).Where(c=>c.CarId ==  id.Key).Where(s => DateTime.Compare((DateTime)s.CompleteDate, (DateTime)dates.Min()) >= 0 && DateTime.Compare((DateTime)s.CompleteDate, ((DateTime)dates.Max()).AddDays(1)) <= 0);
                    DataRow dataRow = dtServices.NewRow();
                    decimal totalAmmount = 0;
                    var car = _context.Cars.Include(c => c.CarModel).ThenInclude(c => c.Manufacturer).Where(c => c.Id == id.Key).FirstOrDefault();
                    dataRow["Гос.номер"] = car.RegistrationNumber;
                    dataRow["Модель"] = car.CarModel.Manufacturer.Name + " " + car.CarModel.Model;


                    foreach (var s in services)
                    {
                        var serDetails = from sd in s.WorkAssigments
                                         group sd by sd.TypeOfService.Service into dsl
                                         select dsl;
                        totalAmmount += (decimal)s.Ammount;

                        foreach (var serv in serDetails)
                        {
                            if (servicesCount.ContainsKey(serv.Key))
                            {
                                servicesCount[serv.Key]++;
                            }
                            else
                            {
                                servicesCount.Add(serv.Key, 1);
                            }
                        }
                        foreach (var item in servicesCount)
                        {
                            dataRow[item.Key] = item.Value;
                        }
                    }

                    dataRow["Итого"] = totalAmmount;
                    dtServices.Rows.Add(dataRow);
                }




                var outpathLocal = Path.Combine(
                Directory.GetCurrentDirectory(), @"wwwroot/OutFiles",
                "Report.xlsx");

               


                dtServices.CreateServiceReport(outpathLocal,dates);

               

            }

            var memory = new MemoryStream();
            var outpath = Path.Combine(
                Directory.GetCurrentDirectory(), @"wwwroot/OutFiles",
                "Report.xlsx");

            using (var stream = new FileStream(outpath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(outpath));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}