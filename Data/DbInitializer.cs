using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;

namespace KernelCars.Data
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            if (context.Cars.Any())
            {
                return;
                //context.Manufacturers.Add(new Manufacturer { Id = 1, Name = "Audi" });
                //context.SaveChanges();
            }
            var cars = new Car[]
            {
                new Car {RegistrationNumber="AX1938BP",FirstRegistrationYear=2012},
                new Car {RegistrationNumber="AA2260OX",FirstRegistrationYear=2016}
                //new Car { ManufacturerId=1, CarTypeId =1,TypeOfFuel=TypeOfFuel.D,RegistrationNumber="AX1938ВР",FirstRegistrationYear=2010,VinNumber="ZZZ",Capacity=1598},
                //new Car { ManufacturerId=4, CarTypeId =8,TypeOfFuel=TypeOfFuel.D,RegistrationNumber="AА2269ОХ",FirstRegistrationYear=2010,VinNumber="ZZZ",Capacity=1598}
                //new Car { Make ="BMW", Type="520",TypeOfFuel=TypeOfFuel.D,RegistrationNumber="AА4804ОХ",FirstRegistrationYear=2010,VinNumber="ZZZ",Capacity=1598},
                //new Car { Make="Audi", Type="A8",TypeOfFuel=TypeOfFuel.D,RegistrationNumber="СА9269ОХ",FirstRegistrationYear=2010,VinNumber="ZZZ",Capacity=1598},
                //new Car { Make="Volkswagen", Type="Passat",TypeOfFuel=TypeOfFuel.D,RegistrationNumber="СА2272ОХ",FirstRegistrationYear=2010,VinNumber="ZZZ",Capacity=1598}
            };
            foreach (Car c in cars)
            {
                context.Cars.Add(c);
            }
            context.SaveChanges();
        }
    }
}
