using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Cluster
    {
        public int Id { get; set; }
        public string ClusterName { get; set; }
        public ICollection<Unit> Units { get; set; }
    }
}
