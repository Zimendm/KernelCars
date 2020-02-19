using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KernelCars.Models;

namespace KernelCars.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }

        DbSet<Car> Cars { get; set; }
    }
}
