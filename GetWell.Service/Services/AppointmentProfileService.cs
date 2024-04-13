using System.Threading.Tasks;
using System.Linq;
using GetWell.Core.Models;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using GetWell.Core.ViewModels;

namespace GetWell.Service.Services
{
	public class AppointmentProfileService : BaseService<AppointmentProfile, Data.Model.AppointmentProfile>, IAppointmentProfileService
	{
        #region ctor

        private readonly IRepository<Data.Model.AppointmentProfile> _repository;

        public AppointmentProfileService(IRepository<Data.Model.AppointmentProfile> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public async Task<PaginatedList<AppointmentProfile>> SearchReviewAsync(ReviewSearch search)
        {
            var query = from appointment in _repository.Context.Appointments
                        join appointmentProfile in _repository.Context.AppointmentProfiles on appointment.ID equals appointmentProfile.AppointmentID
                        join patient in _repository.Context.Patients on appointment.PatientID equals patient.ID
                        join serviceClinicDoctor in _repository.Context.ServiceClinicDoctors on appointment.ServiceClinicDoctorID equals serviceClinicDoctor.ID
                        join serviceClinic in _repository.Context.ServiceClinics on serviceClinicDoctor.ServiceClinicID equals serviceClinic.ID
                        join clinicDoctor in _repository.Context.ClinicDoctors on serviceClinicDoctor.ClinicDoctorID equals clinicDoctor.ID
                        join doctor in _repository.Context.Doctors on clinicDoctor.DoctorID equals doctor.ID
                        join clinic in _repository.Context.Clinics on clinicDoctor.ClinicID equals clinic.ID
                        join service in _repository.Context.Services on serviceClinic.ServiceID equals service.ID
                            where serviceClinic.ClinicID == clinicDoctor.ClinicID && appointment.IsArchived == true
                        orderby appointment.ConfirmationDate descending 
                        select new { appointment, serviceClinic, clinicDoctor, appointmentProfile, service, clinic, doctor, patient };

            if (search.ClinicID > 0)
                query = query.Where(c => c.clinic.ID == search.ClinicID);
            else if (search.ServiceClinicID > 0)
                query = query.Where(c => c.serviceClinic.ID == search.ServiceClinicID);
            else if(search.ClinicDoctorID > 0)
                query = query.Where(c => c.clinicDoctor.ID == search.ClinicDoctorID);
            else if (search.ServiceClinicDoctorID > 0)
                query = query.Where(c => c.appointment.ServiceClinicDoctorID == search.ServiceClinicDoctorID);            

            var result = await query.Select(c => new AppointmentProfile()
            {
                ID = c.appointmentProfile.ID,
                AppointmentID = c.appointment.ID,
                DateCreated = c.appointmentProfile.DateCreated,
                DateUpdated = c.appointmentProfile.DateUpdated,
                PatientReview = c.appointmentProfile.PatientReview,
                PatientRating = c.appointmentProfile.PatientRating,
                ClinicID = c.clinic.ID,
                ClinicName = c.clinic.Name,
                ClinicNameUz = c.clinic.NameUz,
                ClinicNameEn = c.clinic.NameEn,
                DoctorFullName = c.doctor.LastName + " " + c.doctor.FirstName + " " + c.doctor.MiddleName,
                DoctorFullNameUz = c.doctor.LastNameUz + " " + c.doctor.FirstNameUz + " " + c.doctor.MiddleNameUz,
                DoctorFullNameEn = c.doctor.LastNameEn + " " + c.doctor.FirstNameEn + " " + c.doctor.MiddleNameEn,
                ServiceName = c.service.Name,
                ServiceNameUz = c.service.NameUz,
                ServiceNameEn = c.service.NameEn,
                ClinicDoctorID = c.clinicDoctor.ID,
                ServiceClinicID = c.serviceClinic.ID,
                ServiceID = c.service.ID,
                PatientFullName = c.patient.LastName + " " + c.patient.FirstName
            }).GetPaginatedListAsync(search);

            return result;
        }
    }
}