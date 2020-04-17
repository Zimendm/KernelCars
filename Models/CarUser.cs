using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarUser
    {
        public int ID { get; set; }

        public DateTime StartUsingDate { get; set; }
        public DateTime?  EndUsingDate { get; set; }
        public string Comment { get; set; }
        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        
        public long CarId { get; set; }
        public Car Car { get; set; }
    }
}
