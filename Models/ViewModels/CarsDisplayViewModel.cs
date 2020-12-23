using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class CarsDisplayViewModel
    {
        public long Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string CarModel { get; set; }
        public string CarUser { get; set; }
        public string Firm { get; set; }
        public bool EnableService { get; set; }
    }
}
