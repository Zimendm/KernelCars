using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class LeaseListViewModel
    {
        public int Id { get; set; }
        public long CarID { get; set; }
        public string RegistrationNumber { get; set; }
        public string CarModel { get; set; }
        //Арендодатель
        public string LandLord { get; set; }
        //Арендатор
        public string Renter { get; set; }
        public string ContrState { get; set; }
    }
}
