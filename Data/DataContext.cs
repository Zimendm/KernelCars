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
        public DbSet <Unit> Units { get; set; }
        public DbSet <Status> Statuses { get; set; }
        public DbSet<CarStatus> CarStatuses { get; set; }

        public DbSet<CarService> CarServices { get; set; }
        public DbSet<FirmDepartment> FirmDepartment { get; set; }
        public DbSet<TypeOfService> TypeOfServices { get; set; }
        public DbSet<ServiceStation> ServiceStations { get; set; }
        public DbSet<CarUser> CarUsers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LeaseContract> LeaseContracts { get; set; }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Cluster> Clusters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Car>()
            //    .HasOne(a => a.CarOwner)
            //    .WithOne(b => b)
            modelBuilder.Entity<FirmDepartment>()
                .HasKey(c=>new {c.FirmId, c.DepartmentId });

            modelBuilder.Entity<Unit>()
                .HasIndex(u => new { u.DepartmentId, u.FirmId })
                .IsUnique(true);
            modelBuilder.Entity<CarModel>()
                .HasIndex(m => new { m.Model, m.ManufacturerId })
                .IsUnique(true);
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(m => new { m.Name })
                .IsUnique(true);
            modelBuilder.Entity<WorkAssigment>()
                .HasKey(c => new { c.TypeOfServiceId, c.CarServiceID });


            //modelBuilder.Entity<CarStatus>()
            //    .HasOne<Employee>(e => e.Employee)
            //    .WithMany(cs => cs.CarStatuses)
            //    .HasForeignKey(e => e.EmployeeId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        

        
    }
}
