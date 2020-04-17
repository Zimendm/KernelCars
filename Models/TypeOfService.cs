using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class TypeOfService
    {
        public int ID { get; set; }
        public string Service { get; set; }

        public ICollection<WorkAssigment> WorkAssigments { get; set; }
    }
}
