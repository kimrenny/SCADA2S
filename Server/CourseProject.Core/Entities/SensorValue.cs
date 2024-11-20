﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Core.Entities
{
    public class SensorValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime DateTime { get; set; }
        public Sensor Sensor { get; set; }
    }
}