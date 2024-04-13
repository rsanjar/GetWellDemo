
using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicWorkDayService : IBaseService<DTO.ClinicWorkDay>
    {
        Task<List<ClinicWorkDay>> GetAllAsync(int clinicId);
    }
}