﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        [DisplayName("Марка")]
        public string Name { get; set; }
        public List<CarModel> CarModels { get; set; }
    }
}