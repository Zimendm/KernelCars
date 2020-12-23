using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        //Коэффициент для расчёта нормы расхода топлива
        public float RegionCoeff { get; set; }
        public string Comments { get; set; }

        public List<CarStatus> CarStatuses { get; set; }
    }
}
