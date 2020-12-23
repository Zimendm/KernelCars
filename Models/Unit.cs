using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Unit
    {
        public int UnitId { get; set; }
        public bool IsAgriBusiness { get; set; }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "A location must be selected")]
        public int FirmId { get; set; }
        public int DepartmentId { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "A location must be selected")]
        public int ClusterId { get; set; }
        //public int ClasterId { get; set; }
        public Firm Firm { get; set; }
        public Department Department { get; set; }
        public Cluster Cluster { get; set; }
        //public Claster Claster { get; set; }
        public List<CarStatus> CarStatuses { get; set; }

        public string UnitPrintName 
        {
            get
            {
                string retValue = "";
                if (Department!=null&&Firm.Employee!=null)
                {
                    retValue = Department.Name + " [" + Firm.Employee.FullName + "]";
                    if (IsAgriBusiness)
                        retValue += " (Агробизнес)";
                }
                
                return retValue;
            }
        }

    }
}
