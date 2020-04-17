using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class AssignedTypeOfService
    {
        public int TypeOfServiceID { get; set; }
        public string Service { get; set; }
        public bool IsAssigned { get; set; }
    }
}
