using System.Threading.Tasks;
using GetWell.Core.Models;
using Microsoft.AspNetCore.Mvc;
using IBaseModel = GetWell.DTO.Interfaces.IBaseModel;

namespace GetWell.API
{
    public interface IBaseApiReadController<T> where T : class, IBaseModel, new()
    {
        Task<ActionResult<T>> Get(int id);

        Task<ActionResult<PaginatedList<T>>> GetAll(int pageSize = 5, int pageNumber = 1, string orderBy = "ID", bool isAsc = true);
    }
}