using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Core.Entities
{
    public class Sensor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public List<SensorValue> SensorValues { get; set; } = new List<SensorValue>();
    }
}