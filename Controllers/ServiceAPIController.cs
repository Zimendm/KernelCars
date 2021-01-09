using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using KernelCars.Models;
using KernelCars.Data;
using KernelCars.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using KernelCars.Helpers;
using DocumentFormat.OpenXml.Drawing;

namespace KernelCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAPIController : ControllerBase
    {
        private DataContext _context;
        public ServiceAPIController(DataContext ctx)
        {
            _context = ctx;
        }


        [HttpGet]
        public async Task<IEnumerable<ServiceListViewModel>> Get([FromQuery] KernelCars.Models.Shared.PaginationDTO pagination, string searchString = null, string status = null)
        {
            string _searchString = "";
            if (searchString != null)
            {
                _searchString = searchString;
            }

            var quaryable = _context.CarServices
                .Include(c=>c.Car).ThenInclude(c=>c.CarModel).ThenInclude(c=>c.Manufacturer)
                .Include(c => c.Car).ThenInclude(c => c.CarUsers).ThenInclude(c => c.Employee)
                .Include(c => c.ServiceStation);
            //.Include(c => c.Car).ThenInclude(c => c.CarStatuses);

            List<CarService> quaryableList = new List<CarService>();

            //Завершенные
            switch (status)
            {
                case "Открытые":
                    quaryableList = quaryable.Where(c => c.CompleteDate == null).ToList();
                          break;
                case "Завершенные":
                    quaryableList = quaryable.Where(c => c.CompleteDate != null).ToList();
                    break;
                default:
                    quaryableList = quaryable.ToList();
                    break;
            }


            List<ServiceListViewModel> servicesList = new List<ServiceListViewModel>();
            int count = 0;
            //try
            //{
            foreach (var item in quaryableList)
            {
                count++;
                var carUser = (from u in item.Car.CarUsers
                               select u).LastOrDefault();

                //var status = (from s in item.CarStatuses
                //              select s).LastOrDefault();

                string carUserName = carUser != null ? carUser.Employee.LastName+" "+ carUser.Employee.FullName : "Не закреплён";
                string carUserNameToDisplay="";

                if (carUser==null)
                {
                    carUserName = "Не закреплён";
                }
                else
                {
                    string name="";
                    if (carUser.Employee.FirstName.Length>1)
                    {
                        name = carUser.Employee.FirstName.Substring(0, 1) + ".";
                    }
                    string surname="";
                    if (carUser.Employee.MiddleName.Length>1)
                    {
                        surname= carUser.Employee.MiddleName.Substring(0, 1) + ".";
                    }
                    carUserNameToDisplay = $"{carUser.Employee.LastName} {name} {surname}";
                }


                //string carFirmToDisplay = status != null ? status.Unit.Firm.Employee.FullName : "Не указана";
                //bool isEnableService = status != null ? status.IsEnableService : false;
                if (carUserName.ToUpper().IndexOf(_searchString.ToUpper()) >= 0 || item.Car.RegistrationNumber.ToUpper().IndexOf(_searchString.ToUpper()) >= 0)
                {
                    servicesList.Add(
                    new ServiceListViewModel
                    {
                        Id = item.ID,
                        RegistrationNumber = item.Car.RegistrationNumber,
                        CarModel = item.Car.CarModel.Manufacturer.Name + " " + item.Car.CarModel.Model,
                        CarUser = carUserNameToDisplay,
                        ServiceStation=item.ServiceStation.Name,
                        StartDate = item.OpenDate,
                        DateOfCompletion=item.CompleteDate,
                        Comments=item.ServiceDescription
                    });
                }
            }


            await HttpContext.InsertPaginationParameterInResponse(servicesList.AsQueryable(), pagination.QuantityPerPage);
            

            return servicesList.AsQueryable().Paginate(pagination);
        }

    }
}
