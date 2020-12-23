using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public List<Car> Cars { get; set; }
    }
}
