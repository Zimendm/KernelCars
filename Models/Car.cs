﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public enum TypeOfFuel
    {
        Benzine,
        Diesel, 
        Electro
    }
    public enum Transmission
    {
        Автомат,
        Ручная
    }
    public enum DriveType
    {
        Монопривод,
        ПолныйПривод
    }
   
    public class Car
    {
        public long Id { get; set; }

        [DisplayName("Государственный номер")]
        public string RegistrationNumber { get; set; }
        //Тип транспортого средства
        public int? CarTypeId { get; set; }
        public CarType CarType { get; set; }
        //Данный тех.паспорта
        //Номер тех.паспорта



        public string RegistrationCertificate { get; set; }
        //Дата выдачи
        public DateTime RegistrationDate { get; set; }
        //Сервисный центр
        public string Color { get; set; }
        public string TSC { get; set; }
        [DisplayName("Год выпуска")]
        public int FirstRegistrationYear { get; set; }
        [DisplayName("Номер кузова")]
        public string VinNumber { get; set; }
        // Объём двигателя
        [DisplayName("Объём двигателя")]
        public int EngineCapacity { get; set; }
        // Ёмкость бака
        [DisplayName("Топливо")]
        public TypeOfFuel? Fuel { get; set; }
        [DisplayName("Ёмкость бака")]
        public int TankCapacity { get; set; }

        [DisplayName("Линейная норма расхода")]
        public float FuelFlowRate { get; set; }

        //public int SummerTyresId { get; set; }
        //[DisplayName("Размер летних шин")]
        //public TireSize SummerTyres { get; set; }

        //public int WinterTyresId { get; set; }
        //[DisplayName("Размер зимних шин")]
        //public TireSize WinterTyres { get; set; }
        public Transmission? Transmission { get; set; }
        public DriveType? DriveType { get; set; }

        [DisplayName("Размер шин")]
        public int? TyresId { get; set; }
        public TireSize Tyres { get; set; }

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
        public List<LeaseContract> LeaseContracts { get; set; }


        public bool IsPrivat 
        {
            get 
            {
                if (!CarOwner.IsFirm)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool IsRentCar 
        { 
            get 
            {
                try
                {
                    var status = (from s in CarStatuses
                                  select s).LastOrDefault();

                    if (status!=null)
                    {
                        if (CarOwnerId != status.Unit.Firm.EmployeeId)
                        {
                            return true;
                        }
                        
                    }
                    return false;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
        public bool IsEnableService 
        {
            get
            {
                try
                {
                    var status = (from s in CarStatuses
                                  select s).LastOrDefault();

                    if (status != null)
                    {
                        return status.IsEnableService;
                    }
                    return false;
                }
                catch (Exception)
                {

                    return false;
                }
            }
                
        }
        public string DepartmentNameForView 
        {
            get
            {
                var status = (from s in CarStatuses
                              select s).LastOrDefault();

                if (status != null)
                {
                    return status.Unit.Department.Name;
                }
                return "";
            }
        }
        public string LocationForView
        {
            get
            {
                var status = (from s in CarStatuses
                              select s).LastOrDefault();

                if (status != null && status.Location!=null)
                {
                    return status.Location.LocationName;
                }
                return "";
            }
        }
        public string FullNameFirmForView 
        {
            get
            {
                var status = (from s in CarStatuses
                              select s).LastOrDefault();

                if (status != null)
                {
                    return status.Unit.Firm.Employee.FullName;
                    //return status.Unit.Firm.Name;
                }
                return "";
            }
        }
        public string FullFirmName
        {
            get
            {
                var status = (from s in CarStatuses
                              select s).LastOrDefault();

                if (status != null)
                {
                    //return status.Unit.Firm.Employee.FullName;
                    return status.Unit.Firm.Name;
                }
                return "";
            }
        }
        public string CarUserForView
        {
            get
            {
                var carUser = (from u in CarUsers
                              select u).LastOrDefault();

                if (carUser != null)
                {
                    return carUser.Employee.FullName;
                }
                return "";
            }
        }

        public string CarCurrentStatus 
        {
            get 
            {
                var carStatus = (from s in CarStatuses
                                 select s).LastOrDefault();
                if (carStatus!=null)
                {
                    if (carStatus.Status.State!=null)
                    {
                        return carStatus.Status.State;
                    }
                }
                return "Не указан";
            }
        }

        //Вспомогательные временные поля
        public string TempUser { get; set; }
        public string TempModel { get; set; }
        public string TempOwner { get; set; }
        public string TempType { get; set; }
        public string TempInv { get; set; }
        public string TempFirm { get; set; }
    }
}
