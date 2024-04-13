using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicGallery = GetWell.DTO.ClinicGallery;

namespace GetWell.Service.Interface
{
	public interface IClinicGalleryService : IBaseService<ClinicGallery>
    {
        Task<List<ClinicGallery>> GetAllAsync(int clinicId);
    }
}