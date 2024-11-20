using CourseProject.BLL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using CourseProject.API.Controllers;

namespace CourseProject.API.Controllers
{
    public class SensorHub : Hub
    {
        private readonly ISensorService sensorService;

        public SensorHub(ISensorService service)
        {
            this.sensorService = service;
        }

        public async Task SendValue(string id, string value)
        {
            await sensorService.UpdateSensorValue(new()
            {
                Id = Guid.Parse(id),
                Value = value
            });

            await Clients.All.SendAsync("receive", id, value);
        }

        public async Task SendTime()
        {
            var currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            await Clients.All.SendAsync("receiveTime", currentTime);
        }
    }
}
