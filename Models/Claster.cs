using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Claster
    {
        public int Id { get; set; }
        public string ClaserName { get; set; }

        public ICollection<Unit> Units { get; set; }
    }
}
