using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Firm
    {
        public int FirmId { get; set; }
        public int EmployeeId { get; set; }
        
        
        public string Name { get; set; }
        //public int DepartmentId { get; set; }
        //public Department Department { get; set; }

        //public string CEO { get; set; }

        public Employee Employee { get; set; }
        
        public ICollection<Unit> Units { get; set; }
        

    }
}
