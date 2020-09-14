using KernelCars.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Components
{
    public class ServiceSummary : ViewComponent
    {
        private long carId;
        private DataContext _context;
        
        

        public ServiceSummary(DataContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(long carId)
        {
            this.carId = carId;
            var services = new Dictionary<string, int>();
            var odometr = new List<int>();
            float totalAmmount = 0;

            var serviceData = _context.CarServices.Include(s => s.WorkAssigments).ThenInclude(s => s.TypeOfService).Where(c => c.CarId == this.carId);

            //ViewData["TotalAmmount"];

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

                //if (odometr.Max() - odometr.Min() > 0)
                //{
                //    ViewData["Odometr"] = "Пробег за период: " + (odometr.Max() - odometr.Min()).ToString();
                //}
                ViewData["Odometr"] = "Последнее ТО (одометр): " + odometr.Max().ToString();
            }
            ViewData["TotalAmmount"] = totalAmmount;
            //return "From View Component";
            return View(services);
        }
    }
}
