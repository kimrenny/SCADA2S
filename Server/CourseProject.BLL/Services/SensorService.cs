using CourseProject.BLL.Interfaces;
using CourseProject.BLL.Models;
using CourseProject.Core.Entities;
using CourseProject.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Services
{
	public class SensorService : ISensorService
	{
		private readonly CourseProjectDbContext _context;

		public SensorService(CourseProjectDbContext context)
		{
			this._context = context;
		}

		public async Task<Guid> CreateSensor(SensorModel model)
		{
			var sensor = new Sensor()
			{
				Description = model.Description,
				X = model.X,
				Y = model.Y,
				Value = model.Value,
				Unit = model.Unit,
				Name = model.Name
			};

			_context.Add(sensor);
			await _context.SaveChangesAsync();

			return sensor.Id;
		}

		public async Task DeleteSensor(Guid id)
		{
			var sensor = await _context.Sensors.FirstOrDefaultAsync(x => x.Id == id);

			if (sensor is null)
			{
				return;
			}

			_context.Remove(sensor);

			await _context.SaveChangesAsync();
		}

		public async Task<List<SensorModel>> GetSensors()
		{
			var sensors = await _context.Sensors.Include(i => i.SensorValues).ToListAsync();

			var sensorModels = sensors.Select(model => new SensorModel()
			{
				Id = model.Id,
				Description = model.Description,
				Value = model.Value,
				X = model.X,
				Y = model.Y,
				Unit = model.Unit,
				Name = model.Name,
				SensorValues = model.SensorValues.Select(v => v.Value).ToList(),
			}).ToList();

			return sensorModels;
		}

		public async Task UpdateSensorValue(UpdateSensorValueModel model)
		{
			var sensor = await _context.Sensors.FirstOrDefaultAsync(x => x.Id == model.Id);

			if (sensor is null)
			{
				return;
			}

			sensor.Value = model.Value;

			await _context.SaveChangesAsync();
		}
	}
}