﻿// <auto-generated />
using System;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KernelCars.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200414180220_CarUpd")]
    partial class CarUpd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KernelCars.Models.Car", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarModelId")
                        .HasColumnType("int");

                    b.Property<int?>("CarOwnerId")
                        .HasColumnType("int");

                    b.Property<int>("EngineCapacity")
                        .HasColumnType("int");

                    b.Property<int>("FirstRegistrationYear")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImagePage1")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("ImagePage2")
                        .HasColumnType("varbinary(max)");

                    b.Property<bool>("LPG")
                        .HasColumnType("bit");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TankCapacity")
                        .HasColumnType("int");

                    b.Property<string>("TempFirm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempInv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempModel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempOwner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TempUser")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tyres")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VinNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CarModelId");

                    b.HasIndex("CarOwnerId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("KernelCars.Models.CarModel", b =>
                {
                    b.Property<int>("CarModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CarModelId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("Model", "ManufacturerId")
                        .IsUnique()
                        .HasFilter("[Model] IS NOT NULL");

                    b.ToTable("CarModels");
                });

            modelBuilder.Entity("KernelCars.Models.CarService", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CarId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CompleteDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ServiceDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CarId");

                    b.ToTable("CarServices");
                });

            modelBuilder.Entity("KernelCars.Models.CarStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("CarId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CarId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UnitId");

                    b.ToTable("CarStatuses");
                });

            modelBuilder.Entity("KernelCars.Models.CarUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CarId")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndUsingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartUsingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("CarId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("CarUser");
                });

            modelBuilder.Entity("KernelCars.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("KernelCars.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("KernelCars.Models.Firm", b =>
                {
                    b.Property<int>("FirmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FirmId");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("Firms");
                });

            modelBuilder.Entity("KernelCars.Models.FirmDepartment", b =>
                {
                    b.Property<int>("FirmId")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.HasKey("FirmId", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("FirmDepartment");
                });

            modelBuilder.Entity("KernelCars.Models.Manufacturer", b =>
                {
                    b.Property<int>("ManufacturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ManufacturerId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("KernelCars.Models.Status", b =>
                {
                    b.Property<int>("StatusID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusID");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("KernelCars.Models.TypeOfService", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Service")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("TypeOfServices");
                });

            modelBuilder.Entity("KernelCars.Models.Unit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("FirmId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAgriBusiness")
                        .HasColumnType("bit");

                    b.HasKey("UnitId");

                    b.HasIndex("FirmId");

                    b.HasIndex("DepartmentId", "FirmId")
                        .IsUnique();

                    b.ToTable("Units");
                });

            modelBuilder.Entity("KernelCars.Models.WorkAssigment", b =>
                {
                    b.Property<int>("TypeOfServiceId")
                        .HasColumnType("int");

                    b.Property<int>("CarServiceID")
                        .HasColumnType("int");

                    b.HasKey("TypeOfServiceId", "CarServiceID");

                    b.HasIndex("CarServiceID");

                    b.ToTable("WorkAssigment");
                });

            modelBuilder.Entity("KernelCars.Models.Car", b =>
                {
                    b.HasOne("KernelCars.Models.CarModel", "CarModel")
                        .WithMany("Cars")
                        .HasForeignKey("CarModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Employee", "CarOwner")
                        .WithMany()
                        .HasForeignKey("CarOwnerId");
                });

            modelBuilder.Entity("KernelCars.Models.CarModel", b =>
                {
                    b.HasOne("KernelCars.Models.Manufacturer", "Manufacturer")
                        .WithMany("CarModels")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.CarService", b =>
                {
                    b.HasOne("KernelCars.Models.Car", "Car")
                        .WithMany("CarSevices")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.CarStatus", b =>
                {
                    b.HasOne("KernelCars.Models.Car", "Car")
                        .WithMany("CarStatuses")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Unit", "Unit")
                        .WithMany("CarStatuses")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.CarUser", b =>
                {
                    b.HasOne("KernelCars.Models.Car", "Car")
                        .WithMany("CarUsers")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Employee", "Employee")
                        .WithMany("CarUsers")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.Firm", b =>
                {
                    b.HasOne("KernelCars.Models.Employee", "Employee")
                        .WithOne("Firm")
                        .HasForeignKey("KernelCars.Models.Firm", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.FirmDepartment", b =>
                {
                    b.HasOne("KernelCars.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Firm", "Firm")
                        .WithMany()
                        .HasForeignKey("FirmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.Unit", b =>
                {
                    b.HasOne("KernelCars.Models.Department", "Department")
                        .WithMany("Units")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.Firm", "Firm")
                        .WithMany("Units")
                        .HasForeignKey("FirmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KernelCars.Models.WorkAssigment", b =>
                {
                    b.HasOne("KernelCars.Models.CarService", "CarService")
                        .WithMany("WorkAssigments")
                        .HasForeignKey("CarServiceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KernelCars.Models.TypeOfService", "TypeOfService")
                        .WithMany("WorkAssigments")
                        .HasForeignKey("TypeOfServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
