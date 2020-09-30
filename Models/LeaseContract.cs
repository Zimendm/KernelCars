using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public enum Currency
    {
        EUR,
        USD,
        UAH
    }
    public class LeaseContract
    {
        public int ID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = new DateTime(DateTime.Now.Year, 6, 30);
        public decimal LeaseAmmount { get; set; }


        

        public long CarId { get; set; }
        public Car Car { get; set; }

        //Арендатор
        //public int? TenantId { get; set; }
        //public Employee Tenant { get; set; }
    }
}
