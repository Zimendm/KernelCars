using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class WorkAssigment
    {
        public int CarServiceID { get; set; }
        public CarService CarService { get; set; }
        public int TypeOfServiceId { get; set; }
        public TypeOfService TypeOfService { get; set; }
    }
}
