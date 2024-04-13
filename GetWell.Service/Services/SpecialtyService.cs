using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class SpecialtyService : BaseService<Specialty, Data.Model.Specialty>, ISpecialtyService
	{
		public SpecialtyService(IRepository<Data.Model.Specialty> repository) : base(repository)
		{
		}
	}
}