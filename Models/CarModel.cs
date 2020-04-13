using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarModel
    {
        public int CarModelId { get; set; }
        public string Model { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public List<Car> Cars { get; set; }
    }
}
