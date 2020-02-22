using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public enum TypeOfFuel
    {
        B, D, E
    }

    public class Car
    {
        public long Id { get; set; }

            //[DisplayName("Модель")]
            //public string Make { get; set; }
            //public string Type { get; set; }
        public string RegistrationNumber { get; set; }
        public int FirstRegistrationYear { get; set; }
        public string VinNumber { get; set; }

        // Объём двигателя
        public int EngineCapacity { get; set; }
        // Ёмкость бака
        public int TankCapacity { get; set; }


        //public TypeOfFuel TypeOfFuel { get; set; }
        //[DisplayName("Марка")]
        //public int ManufacturerId { get; set; }
        //public Manufacturer Manufacturer { get; set; }

        //[DisplayName("Модель")]
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }
    }
}
