using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarStatus
    {
        public int ID { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        public long CarId { get; set; }
        public Car Car { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        //public int EmployeeId { get; set; }
        //public Employee Employee { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public string Comment { get; set; }
    }
}
