using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.API.Models;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetWell.API.Controllers
{
	public class ClinicDoctorWorkDayController :  BaseApiController<ClinicDoctorWorkDay>
	{
		#region ctor

		private readonly IClinicDoctorWorkDayService _service;
		private readonly IAppointmentService _appointmentService;

		public ClinicDoctorWorkDayController(IClinicDoctorWorkDayService service, 
			IAppointmentService appointmentService, PatientCacheHelper patientCacheHelper) : base(service, patientCacheHelper)
		{
			_service = service;
			_appointmentService = appointmentService;
		}

		#endregion

		[AllowAnonymous]
		[HttpGet("getall/{clinicDoctorID:int}")]
		public async Task<ActionResult<List<ClinicDoctorWorkDay>>> GetAllAsync(int clinicDoctorID)
		{
			var result = await _service.GetAllAsync(clinicDoctorID);

			return Ok(result);
		}

		[AllowAnonymous]
		[HttpGet("get-doctor-time-slots/{clinicDoctorID:int}")]
		public async Task<ActionResult<List<DoctorHourSlot>>> GetDoctorTimeSlots(int clinicDoctorID, DateTime date, int slotMinutes = 5, bool includeOccupiedSlots = false)
		{
			var workDay = await _service.GetAsync(clinicDoctorID, date) ?? new ClinicDoctorWorkDay();
			var hours = new List<DoctorHourSlot>();
			
			for (int hour = (int)Math.Floor(workDay.StartTime.TotalHours); hour < (int)Math.Ceiling(workDay.EndTime.TotalHours); hour++)
			{
				var hourSlot = new DoctorHourSlot(hour);

				for (int i = 0; i < 60; i += GetSlotMinutes(slotMinutes))
				{
					var currentTime = new TimeSpan(hour, i, 0);
                    bool isOccupied = IsInBreakTime(workDay, currentTime) || 
                                      await IsAnyAppointments(clinicDoctorID, date, currentTime);

                    if((includeOccupiedSlots && isOccupied) || !isOccupied)
                        hourSlot.TimeSlots.Add(new DoctorMinuteSlot(isOccupied, i));
                }

				if(hourSlot.TimeSlots.Any())
				    hours.Add(hourSlot);
			}

			return Ok(hours);
		}

		private bool IsInBreakTime(ClinicDoctorWorkDay workDay, TimeSpan timeSlot)
		{
			if (workDay.BreakEndTime == null || workDay.BreakStartTime == null)
				return false;

			if(timeSlot >= workDay.BreakStartTime && timeSlot < workDay.BreakEndTime)
				return true;

			return false;
		}

        private async Task<bool> IsAnyAppointments(int clinicDoctorID, DateTime date, TimeSpan currentTime)
        {
            var appointments = await _appointmentService.GetAllAsync(clinicDoctorID, date);

            return appointments.Any(a => currentTime >= a.AppointmentTime && currentTime <= a.EndTime);
        }

		private int GetSlotMinutes(int slotMinutes)
		{
			if (slotMinutes % 5 != 0 || slotMinutes > 30 || slotMinutes < 5)
				slotMinutes = 5;

			return slotMinutes;
		}
	}
}