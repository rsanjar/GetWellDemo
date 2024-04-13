using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;

namespace GetWell.Service.Services
{
	public class WeekDayService : BaseService<WeekDay, Data.Model.WeekDay>, IWeekDayService
	{
		public WeekDayService(IRepository<Data.Model.WeekDay> repository) : base(repository)
		{
		}
	}
}