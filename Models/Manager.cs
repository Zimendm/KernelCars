using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Manager
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        //Адрес прописки
        public string Registration { get; set; }
        //Город
        public string City { get; set; }

        //Доверенность на подписание договоров аренды
        public string LeaseProcuratoryNumber { get; set; }
        public DateTime LeaseProcuratoryDate { get; set; }
        public string LeaseProcuratory { get; set; }

        //Navigation properties
        public List<LeaseContract> LeaseContracts { get; set; }
    }
}
