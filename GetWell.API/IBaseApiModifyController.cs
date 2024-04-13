using System.Threading.Tasks;
using GetWell.Core;
using Microsoft.AspNetCore.Mvc;
using IBaseModel = GetWell.DTO.Interfaces.IBaseModel;

namespace GetWell.API
{
    public interface IBaseApiModifyController<T> where T : class, IBaseModel, new()
    {
        Task<ActionResult<CrudResponse>> Add(T model);

        Task<ActionResult<CrudResponse>> Save(int id, T model);

        Task<ActionResult<CrudResponse>> Delete(int id);
    }
}