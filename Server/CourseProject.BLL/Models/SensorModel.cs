using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Models
{
    public class SensorModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        public string Value {  get; set; }
        public string? Unit { get; set; }
        public List<string> SensorValues { get; set; } = new List<string>();
    }
}