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
    public class CarAPIController : ControllerBase
    {
        private DataContext _context;

        public CarAPIController(DataContext ctx)
        {
            _context = ctx;
        }

        //[HttpGet]
        //public IEnumerable<KernelCars.Models.Location> GetProducts()
        //{
        //    return new KernelCars.Models.Location[] {
        //        new KernelCars.Models.Location() { LocationName = "Location #1" },
        //        new KernelCars.Models.Location() { LocationName = "Location #2" },
        //    };
        //}

        [HttpGet]
        public async Task<IEnumerable<CarsDisplayViewModel>> Get([FromQuery] KernelCars.Models.Shared.PaginationDTO pagination, string searchString=null)
        {
            string _searchString = "";
            if (searchString != null)
            {
                _searchString = searchString;
            }

            var quaryable = _context.Cars
                .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
                .Include(c => c.CarUsers).ThenInclude(c => c.Employee)
                .Include(c => c.CarStatuses).ThenInclude(c => c.Unit).ThenInclude(c => c.Firm).ThenInclude(c => c.Employee);
            //.Where(c => c.RegistrationNumber.IndexOf(_searchString) > -1  );
            //_context.Cars.AsQueryable();


            //await HttpContext.InsertPaginationParameterInResponse(quaryable. double count = await queyable.CountAsync();, pagination.QuantityPerPage);

            //var carsQuery = quaryable.Paginate(pagination);


            ///_context.Cars
            //    .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
            //    .Include(c=>c.CarUsers).ThenInclude(c=>c.Employee)
            //    .Include(c=>c.CarStatuses).ThenInclude(c=>c.Unit).ThenInclude(c=>c.Firm).ThenInclude(c=>c.Employee).Where(c=>c.RegistrationNumber.IndexOf(searchString)>-1)
            //    .Paginate(pagination);

            List<CarsDisplayViewModel> carsList = new List<CarsDisplayViewModel>();
            int count = 0;
            //try
            //{
                foreach (var item in quaryable)
                {
                    count++;
                    var carUser = (from u in item.CarUsers
                                   select u).LastOrDefault();
                    var status = (from s in item.CarStatuses
                                  select s).LastOrDefault();

                    string carUserName = carUser != null ? carUser.Employee.FullName : "Не закреплён";
                    string carFirmToDisplay = status != null ? status.Unit.Firm.Employee.FullName : "Не указана";
                    bool isEnableService = status != null ? status.IsEnableService : false;
                if (carUserName.ToUpper().IndexOf(_searchString.ToUpper()) >= 0 || item.RegistrationNumber.ToUpper().IndexOf(_searchString.ToUpper()) >= 0)
                {
                    carsList.Add(
                    new CarsDisplayViewModel
                    {
                        Id = item.Id,
                        RegistrationNumber = item.RegistrationNumber,
                        CarModel = item.CarModel.Manufacturer.Name + " " + item.CarModel.Model,
                        CarUser = carUserName,
                        Firm = carFirmToDisplay,
                        EnableService = isEnableService
                    });
                }
                }


            //}
            //catch (Exception)
            //{

            //    throw;
            //}


            await HttpContext.InsertPaginationParameterInResponse(carsList.AsQueryable(), pagination.QuantityPerPage);
            //var carsList = carsList.AsQueryable().Paginate(pagination);



            return carsList.AsQueryable().Paginate(pagination);
        }

        //[HttpGet]
        //public async Task<IEnumerable<Car>> Get()
        //{
        //    return await _context.Cars
        //        .Include(c => c.CarModel).ThenInclude(c => c.Manufacturer)
        //        .Include(c => c.CarUsers).ThenInclude(c => c.Employee)
        //        .Include(c => c.CarStatuses).ThenInclude(c => c.Status)
        //        .Include(c=>c.CarUsers).ThenInclude(c=>c.Employee).ToListAsync();
        //}


        [HttpGet("{id}")]
        public KernelCars.Models.Location GetProduct()
        {
            return new KernelCars.Models.Location()
            {
                ID = 1,
                LocationName = "Test Location"
            };
        }

        
    }
}
