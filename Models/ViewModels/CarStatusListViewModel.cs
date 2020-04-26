using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class CarStatusListViewModel
    {
        public IEnumerable<CarStatusViewModel> CarStatuses { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentStatus { get; set; }
    }
}
