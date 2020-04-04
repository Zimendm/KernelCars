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
        public DataContext(DbContextOptions<DataContext> options)
            :base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet <Firm> Firms { get; set; }
        public DbSet <Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Car>()
            //    .HasOne(a => a.CarOwner)
            //    .WithOne(b => b)
            modelBuilder.Entity<FirmDepartment>()
                .HasKey(c=>new {c.FirmId, c.DepartmentId });
        }

        public DbSet<KernelCars.Models.FirmDepartment> FirmDepartment { get; set; }
    }
}
