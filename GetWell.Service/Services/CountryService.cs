using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class CountryService : BaseService<Country, Data.Model.Country>, ICountryService
	{
		public CountryService(IRepository<Data.Model.Country> repository) : base(repository)
		{
		}
	}
}