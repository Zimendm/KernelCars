using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class CarService
    {
        public int ID { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int Odometr { get; set; }
        public float Ammount { get; set; }
        public string ServiceDescription { get; set; }

        public long CarId { get; set; }
        public Car Car { get; set; }
                     
        public int ServiceStationID { get; set; }
        public ServiceStation ServiceStation { get; set; }

        //public List<TypeOfService> PlannedWork { get; set; }
        //public ICollection<TypeOfService> Works { get; set; }
        public ICollection<WorkAssigment> WorkAssigments { get; set; }
        //TODO Добавить менеджера, станцию и возможность сохранять скан-копии в формате .pdf
        
        public string DocumentPath { get; set; }
    }
}
