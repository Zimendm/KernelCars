using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarStatus
    {
        public int ID { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEnableService { get; set; }

        public long CarId { get; set; }
        public Car Car { get; set; }
        
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "A status must be selected")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "A location must be selected")]
        public int LocationId { get; set; }
        public Location Location { get; set; }
        //public int EmployeeId { get; set; }
        //public Employee Employee { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "A status must be selected")]
        public int UnitId { get; set; }
        [ValidateComplexType]
        public Unit Unit { get; set; }
        public string Comment { get; set; }
    }
}
