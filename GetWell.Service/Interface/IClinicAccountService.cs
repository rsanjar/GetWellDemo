using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface
{
	public interface IClinicAccountService : IBaseService<DTO.ClinicAccount>, IAuthenticatable
    {
        Task<ClinicAccount> GetByClinicAsync(int clinicId);
    }
}