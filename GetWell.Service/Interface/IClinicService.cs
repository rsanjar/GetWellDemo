using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicService : IBaseService<Clinic>
	{
		Task<PaginatedList<Clinic>> GetAllAsync(PaginatedList<Clinic> pagination, int cityID = 0, int patientID = 0);
		
        Task<PaginatedList<Clinic>> GetAllAsync(int serviceCategoryID, PaginatedList<Clinic> pagination, int patientID = 0);

        Task<PaginatedList<Clinic>> SearchAsync(int serviceID, PaginatedList<Clinic> pagination, int patientID = 0);

        Task<PaginatedList<Clinic>> GetAllAsync(ClinicSearch search);

        Task<PaginatedList<Clinic>> GetAllByCategoryAsync(int categoryID, PaginatedList<Clinic> pagination, int cityID = 0, int patientID = 0);

        Task<List<Clinic>> GetAllByCityAsync(int cityID);

        Task<List<Clinic>> GetAllAsync(int regionID);
        
        Task<Clinic> GetAsync(Guid uniqueKey);

        Task<string> GetQrImageBase64Async(int clinicID);

        Task<CrudResponse> SaveQrImageBase64Async(int clinicID, string base64Image);

        Task<Guid> GetUniqueKey(int clinicID);

        Task<PaginatedList<Clinic>> AutoCompleteSearchAsync(string term, int pageSize = 5);
    }
}