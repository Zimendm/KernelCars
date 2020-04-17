using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public bool IsAgriBusiness { get; set; } 
        public int FirmId { get; set; }
        public int DepartmentId { get; set; }
        public Firm Firm { get; set; }
        public Department Department { get; set; }
        public List<CarStatus> CarStatuses { get; set; }


    }
}
