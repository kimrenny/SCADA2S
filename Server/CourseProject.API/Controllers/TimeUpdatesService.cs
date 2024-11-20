using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace CourseProject.API.Controllers
{
    public class TimeUpdatesService : BackgroundService
    {
        private readonly IHubContext<SensorHub> _hubContext;

        public TimeUpdatesService(IHubContext<SensorHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                await _hubContext.Clients.All.SendAsync("receiveTime", currentTime);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
