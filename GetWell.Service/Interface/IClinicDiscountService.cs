using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.DTO;

namespace GetWell.Service.Interface;

public interface IClinicDiscountService : IBaseService<ClinicDiscount>
{
	Task<List<ClinicDiscount>> GetAllActive();
}