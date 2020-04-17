using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Государственный номер")]
        public string RegistrationNumber { get; set; }
        [DisplayName("Год выпуска")]
        public int FirstRegistrationYear { get; set; }
        [DisplayName("Номер кузова")]
        public string VinNumber { get; set; }
        // Объём двигателя
        [DisplayName("Объём двигателя")]
        public int EngineCapacity { get; set; }
        // Ёмкость бака
        [DisplayName("Ёмкость бака")]
        public int TankCapacity { get; set; }
        [DisplayName("Размер шин")]
        public string Tyres { get; set; }
        public bool LPG { get; set; }
        public byte[] ImagePage1 { get; set; }
        public byte[] ImagePage2 { get; set; }

        //Navigation properties
        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }
        
        public int? CarOwnerId { get; set; }
        public Employee CarOwner  { get; set; }
        public List<CarStatus> CarStatuses { get; set; }
        public List<CarUser> CarUsers { get; set; }
        public List<CarService> CarSevices { get; set; }


        //Вспомогательные временные поля
        public string TempUser { get; set; }
        public string TempModel { get; set; }
        public string TempOwner { get; set; }
        public string TempType { get; set; }
        public string TempInv { get; set; }
        public string TempFirm { get; set; }
    }
}
