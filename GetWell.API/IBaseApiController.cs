using IBaseModel = GetWell.DTO.Interfaces.IBaseModel;

namespace GetWell.API
{
	public interface IBaseApiController<T> : IBaseApiReadController<T>, IBaseApiModifyController<T> where T : class, IBaseModel, new()
	{

	}
}