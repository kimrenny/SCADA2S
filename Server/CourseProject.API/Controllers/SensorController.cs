using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CourseProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _service;
        private readonly IHubContext<SensorHub> _hubContext;

        public SensorController(ISensorService service, IHubContext<SensorHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetSensors()
        {
            var sensors = await _service.GetSensors();

            return Ok(sensors);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSensor(SensorModel model)
        {
            var id = await _service.CreateSensor(model);

            return Ok(id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSensorValue(UpdateSensorValueModel model)
        {
            await _service.UpdateSensorValue(model);
            await _hubContext.Clients.All.SendAsync("receive", model.Id.ToString(), model.Value);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSensor(Guid id)
        {
            await _service.DeleteSensor(id);

            return NoContent();
        }
    }
}