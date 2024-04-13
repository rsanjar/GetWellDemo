using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class ZipCodeService : BaseService<ZipCode, Data.Model.ZipCode>, IZipCodeService
	{
		public ZipCodeService(IRepository<Data.Model.ZipCode> repository) : base(repository)
		{
		}
	}
}