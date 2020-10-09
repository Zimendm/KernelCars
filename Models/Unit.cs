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
        //public int ClasterId { get; set; }
        public Firm Firm { get; set; }
        public Department Department { get; set; }
        //public Claster Claster { get; set; }
        public List<CarStatus> CarStatuses { get; set; }

        public string UnitPrintName 
        {
            get
            {
                string retValue = Department.Name + " [" + Firm.Employee.FullName + "]";
                if (IsAgriBusiness)
                    retValue += " (Агробизнес)";
                return retValue;
            }
        }

    }
}
