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
            if (context.Manufacturers.Any())
            {
                return;
            }

            var manufacturars = new Manufacturer[]
            {
                new Manufacturer{ Name = "Mercedes-Benz" },
                new Manufacturer{ Name = "Audi" },
                new Manufacturer{ Name = "Bmw" },
                new Manufacturer{ Name = "Volkswagen" }
            };
            foreach (Manufacturer m in manufacturars)
            {
                context.Manufacturers.Add(m);
            }
            context.SaveChanges();
            var models = new CarModel[]
            {
                new CarModel {ManufacturerId=1,Model="G 63 AMG"},
                new CarModel {ManufacturerId=1,Model="GLE 43 AMG"},
                new CarModel {ManufacturerId=1,Model="GLC 250 4 Matic"},
                new CarModel {ManufacturerId=2,Model="A8"}
            };
            foreach (CarModel carModel in models)
            {
                context.CarModels.Add(carModel);
            }
            context.SaveChanges();

            var carOwners = new Employee[]
            {
                new Employee {FirstName="АТП-2004"},
                new Employee {FirstName="КЕРНЕЛ-ТРЕЙД ТОВ"}
            };
            foreach (Employee item in carOwners)
            {
                context.Employees.Add(item);
            }
            context.SaveChanges();


            var cars = new Car[]
            {
                new Car {RegistrationNumber="AX1938BP",FirstRegistrationYear=2012, VinNumber="WDD10000000", CarModelId=1, CarOwnerId=1},
                new Car {RegistrationNumber="AA2269OX",FirstRegistrationYear=2016, VinNumber="WDD10000001", CarModelId=2, CarOwnerId=1},
                new Car {RegistrationNumber="AA8400OX",FirstRegistrationYear=2019, VinNumber="WDD10000002", CarModelId=2, CarOwnerId=1},
                new Car {RegistrationNumber="AA8422II",FirstRegistrationYear=2017, VinNumber="WDD10000003", CarModelId=3, CarOwnerId=2}
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
