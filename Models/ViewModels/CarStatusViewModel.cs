using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models.ViewModels
{
    public class CarStatusViewModel : CarStatus
    {
        public bool IsPrivat { get; set; }
        public bool IsRentCar { get; set; }

        public CarStatusViewModel(CarStatus carStatus)
        {
            ID = carStatus.ID;
            BeginDate = carStatus.BeginDate;
            EndDate = carStatus.EndDate;
            CarId = carStatus.CarId;
            Car = carStatus.Car;
            StatusId = carStatus.StatusId;
            Status = carStatus.Status;
            LocationId = carStatus.LocationId;
            Location = carStatus.Location;
            UnitId = carStatus.UnitId;
            Unit = carStatus.Unit;
            Comment = carStatus.Comment;
            IsEnableService = carStatus.IsEnableService;

            if (!carStatus.Car.CarOwner.IsFirm)
            {
                IsPrivat = true;
            }
            if (Car.CarOwnerId!=Unit.Firm.EmployeeId)
            {
                IsRentCar = true;
            }

        }
    }
}
