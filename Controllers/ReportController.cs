using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}