using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string FullName {
            get
            {
                return (LastName + " " + FirstName + " " + MiddleName).Trim();
            }
        }

        public string Address { get; set; }
        public string TaxNumber { get; set; }

        public bool IsFirm { 
            get
            {
                if (FirstName==null && MiddleName==null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Firm Firm { get; set; }
        public List <CarUser> CarUsers { get; set; }
        //public List<CarStatus> CarStatuses { get; set; }

        //TODO для заключения внутригрупповых договоров аренды и других добавить банковские реквизиты и другие данные.
    }
}
