using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public enum Currency
    {
        UAH,
        USD,
        EUR
    }
    public class LeaseContract
    {
        public int ID { get; set; }

        //Срок действия договора аренды
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } = new DateTime(DateTime.Now.Year, 6, 30);

        // Даты приёма/возврата автомобиля
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } 

        
        [Column(TypeName = "decimal(5, 2)")]
        public decimal LeaseAmmount { get; set; }

        public Currency LeaseCurrency { get; set; }



        public long CarId { get; set; }
        public Car Car { get; set; }

        public int ManagerId { get; set; }
        public Manager Manager { get; set; }
        //Арендатор
        //public int? TenantId { get; set; }
        //public Employee Tenant { get; set; }
    }
}
