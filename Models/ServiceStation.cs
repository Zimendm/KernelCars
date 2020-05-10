using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class ServiceStation
    {
        public int ID { get; set; }
        public string OKPO { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }

        public List<CarService> CarServices { get; set; }
    }
}
