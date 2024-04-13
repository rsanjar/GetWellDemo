using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IAppointmentService : IBaseService<DTO.Appointment>
    {
        Task<List<Appointment>> GetAllAsync(AppointmentSearch search);

        Task<PaginatedList<Appointment>> GetAllAsync(int patientID, PaginationModel<Appointment> pagination);

        Task<Appointment> GetDetailsAsync(int id);

        Task<List<Appointment>> GetAllAsync(int clinicDoctorID, DateTime appointmentDate);

        Task<CrudResponse> AddReviewAsync(Appointment appointment);

        Task<int> GetCountByClinic(int clinicID);

        Task<int> GetCountByClinicDoctor(int clinicDoctorID);

        Task<CrudResponse> CancelAppointmentAsync(int id, int patientID);

        Task<string> GetQrImageBase64Async(int id, int patientID);

        Task<CrudResponse> SetArchived(Guid code);

        Task<int> GetID(Guid code);
    }
}