using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Models
{
    public class UpdateSensorValueModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
    }
}