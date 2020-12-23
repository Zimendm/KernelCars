using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class TireSize
    {
        public int Id { get; set; }
        public string Size { get; set; }

        public List<Car> Cars { get; set; }
    }
}
