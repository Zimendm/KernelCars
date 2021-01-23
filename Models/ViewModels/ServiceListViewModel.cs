using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class ServiceListViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string CarModel { get; set; }
        public string CarUser { get; set; }
        public string ServiceStation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? DateOfCompletion { get; set; }
        public string Comments { get; set; }
        public int Odometer { get; set; }
        public float Ammount { get; set; }
        //public string Firm { get; set; }
    }
}
