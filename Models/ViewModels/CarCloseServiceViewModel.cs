using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class CarCloseServiceViewModel
    {
        public int CarServiceId { get; set; }
        public CarService CarService { get; set; }
        //public DateTime CloseDate { get; set; }
        public IFormFile DocumentScan { get; set; }
    }
}
