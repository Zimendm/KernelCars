using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public bool IsAgriBusines { get; set; }

        //public List<Firm> Firms { get; set; }
    }
}
