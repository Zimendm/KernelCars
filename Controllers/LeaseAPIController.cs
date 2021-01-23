using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KernelCars.Models;
using KernelCars.Data;
using KernelCars.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using KernelCars.Helpers;
using KernelCars.Models;

namespace KernelCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaseAPIController : ControllerBase
    {
        private DataContext _context;

        public LeaseAPIController(DataContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        public async Task<IEnumerable<LeaseListViewModel>> Get([FromQuery] KernelCars.Models.Shared.PaginationDTO pagination, string searchString = null, string contrstatus = null)
        {
            string _searchString = "";
            string _contrstatus; //= "Отсутствующие"; 
            int leaseContractId = 0; ;

            if (searchString != null)
            {
                _searchString = searchString;
            }
            if (contrstatus!=null)
            {
                _contrstatus = contrstatus;
            }
            else
            {
                _contrstatus = "Отсутствующие";
            }


            var carNeedLeaseContract = _context.Cars
                .Include(c=>c.CarOwner)
                .Include(c=>c.CarModel).ThenInclude(cm=>cm.Manufacturer)
                .Include(c=>c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);



            List<Car> carNeedLeaseContractList = new List<Car>();
            List<LeaseListViewModel> carsList = new List<LeaseListViewModel>();


            foreach (var item in carNeedLeaseContract)
            {
                if (item.IsRentCar)
                {
                    string contractState= "Не заключён";

                    var leaseQuery = _context.LeaseContracts
                        .Include(c => c.Car).Where(lc => lc.CarId == item.Id).ToArray();

                    if (leaseQuery!=null && leaseQuery.Count()>0)
                    {
                        if (leaseQuery.Last().EndDate != null)
                        {
                            if (((DateTime)leaseQuery.Last().EndDate).CompareTo(DateTime.Now) <= 0 )
                            {
                                contractState = $"Не действует с {((DateTime)leaseQuery.Last().EndDate).ToShortDateString()}";
                                leaseContractId = leaseQuery.Last().ID;
                            }
                            else
                            {
                                contractState = $"Действует до {((DateTime)leaseQuery.Last().EndDate).ToShortDateString()}";
                                leaseContractId = leaseQuery.Last().ID;
                            }
                        }
                        else
                        {
                            if (((DateTime)leaseQuery.Last().ToDate).CompareTo(DateTime.Now) <= 0)
                            {
                                contractState = $"Не действует с {((DateTime)leaseQuery.Last().ToDate).ToShortDateString()}";
                                leaseContractId = leaseQuery.Last().ID;
                            }
                            else
                            {
                                contractState = $"Действует до {((DateTime)leaseQuery.Last().ToDate).ToShortDateString()}";
                                leaseContractId = leaseQuery.Last().ID;
                            }
                        }
                    }
                    
                    var status = (from s in item.CarStatuses
                                  select s).LastOrDefault();
                    string carFirmToDisplay = status != null ? status.Unit.Firm.Employee.FullName : "Не указана";

                    switch (_contrstatus)
                    {
                        case "Отсутствующие":
                            if (contractState == "Не заключён")
                            {
                                carsList.Add(new LeaseListViewModel
                                {
                                    CarID=item.Id,
                                    RegistrationNumber = item.RegistrationNumber,
                                    CarModel = $"{item.CarModel.Manufacturer.Name} {item.CarModel.Model}",
                                    LandLord = item.CarOwner.FullName,
                                    Renter = carFirmToDisplay,
                                    ContrState = contractState
                                });
                            }
                            break;
                        case "Действующие":
                            if (contractState.IndexOf("Действует до") ==0)
                            {
                                carsList.Add(new LeaseListViewModel
                                {
                                    Id=leaseContractId,
                                    CarID = item.Id,
                                    RegistrationNumber = item.RegistrationNumber,
                                    CarModel = $"{item.CarModel.Manufacturer.Name} {item.CarModel.Model}",
                                    LandLord = item.CarOwner.FullName,
                                    Renter = carFirmToDisplay,
                                    ContrState = contractState
                                });
                            }
                            break;
                        case "Закрытые":
                            if (contractState.IndexOf("Не действует с") == 0)
                            {
                                carsList.Add(new LeaseListViewModel
                                {
                                    Id = leaseContractId,
                                    CarID = item.Id,
                                    RegistrationNumber = item.RegistrationNumber,
                                    CarModel = $"{item.CarModel.Manufacturer.Name} {item.CarModel.Model}",
                                    LandLord = item.CarOwner.FullName,
                                    Renter = carFirmToDisplay,
                                    ContrState = contractState
                                });
                            }
                            break;
                        default:
                            carsList.Add(new LeaseListViewModel
                            {
                                Id = leaseContractId,
                                CarID = item.Id,
                                RegistrationNumber = item.RegistrationNumber,
                                CarModel = $"{item.CarModel.Manufacturer.Name} {item.CarModel.Model}",
                                LandLord = item.CarOwner.FullName,
                                Renter = carFirmToDisplay,
                                ContrState = contractState
                            });
                            break;
                    }


                    
                }
            }

            //var quaryable = _context.LeaseContracts
            //    .Include(lc=>lc.Car)
            //    .ThenInclude(lc=>lc.CarOwner)
            //    .Include(lc=>lc.Car).ThenInclude(c=>c.CarModel).ThenInclude(cm=>cm.Manufacturer)
            //    //.Include(lc=>lc.Car).ThenInclude(c=>c.CarUsers).ThenInclude(cu=>cu.Employee)
            //    .Include(lc => lc.Car).ThenInclude(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee); ;
            
            //.Cars
            //    .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
            //    .Include(c => c.CarUsers).ThenInclude(c => c.Employee)
            //    .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);
            ////.Where(c => c.RegistrationNumber.IndexOf(_searchString) > -1  );
            //_context.Cars.AsQueryable();


            //await HttpContext.InsertPaginationParameterInResponse(quaryable. double count = await queyable.CountAsync();, pagination.QuantityPerPage);

            //var carsQuery = quaryable.Paginate(pagination);


            ///_context.Cars
            //    .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
            //    .Include(c=>c.CarUsers).ThenInclude(c=>c.Employee)
            //    .Include(c=>c.CarStatuses).ThenInclude(c=>c.Unit).ThenInclude(c=>c.Firm).ThenInclude(c=>c.Employee).Where(c=>c.RegistrationNumber.IndexOf(searchString)>-1)
            //    .Paginate(pagination);

            //List<LeaseListViewModel> carsList = new List<LeaseListViewModel>();
            ////int count = 0;
            //foreach (var item in quaryable)
            //{
            //    //count++;

            //    var status = (from s in item.Car.CarStatuses
            //                  select s).LastOrDefault();

            //    string carFirmToDisplay = status != null ? status.Unit.Firm.Employee.FullName : "Не указана";
            //    string carModel = $"{item.Car.CarModel.Manufacturer.Name} {item.Car.CarModel.Model}";

            //    //if (carUserName.ToUpper().IndexOf(_searchString.ToUpper()) >= 0 || item.Car.RegistrationNumber.ToUpper().IndexOf(_searchString.ToUpper()) >= 0)
            //    if (item.Car.RegistrationNumber.ToUpper().IndexOf(_searchString.ToUpper()) >= 0)
            //    {


            //        carsList.Add(new LeaseListViewModel
            //        {
            //            Id = item.ID,
            //            RegistrationNumber = item.Car.RegistrationNumber,
            //            CarModel=carModel,
            //            LandLord=item.Car.CarOwner.FullName,
            //            Renter=carFirmToDisplay
            //        });
            //    }
            //}
            //try
            //{
            //////////////foreach (var item in quaryable)
            //////////////{
            //////////////    count++;
            //////////////    var carUser = (from u in item.CarUsers
            //////////////                   select u).LastOrDefault();
            //////////////    var status = (from s in item.CarStatuses
            //////////////                  select s).LastOrDefault();

            //////////////    string carUserName = carUser != null ? carUser.Employee.FullName : "Не закреплён";
            //////////////    string carFirmToDisplay = status != null ? status.Unit.Firm.Employee.FullName : "Не указана";
            //////////////    bool isEnableService = status != null ? status.IsEnableService : false;
            //////////////    if (carUserName.ToUpper().IndexOf(_searchString.ToUpper()) >= 0 || item.RegistrationNumber.ToUpper().IndexOf(_searchString.ToUpper()) >= 0)
            //////////////    {
            //////////////        carsList.Add(
            //////////////        new CarsDisplayViewModel
            //////////////        {
            //////////////            Id = item.Id,
            //////////////            RegistrationNumber = item.RegistrationNumber,
            //////////////            CarModel = item.CarModel.Manufacturer.Name + " " + item.CarModel.Model,
            //////////////            CarUser = carUserName,
            //////////////            Firm = carFirmToDisplay,
            //////////////            EnableService = isEnableService
            //////////////        });
            //////////////    }
            //////////////}


            //}
            //catch (Exception)
            //{

            //    throw;
            //}


            await HttpContext.InsertPaginationParameterInResponse(carsList.AsQueryable(), pagination.QuantityPerPage);
            //var carsList = carsList.AsQueryable().Paginate(pagination);



            return carsList.AsQueryable().Paginate(pagination);
        }




    }
}
