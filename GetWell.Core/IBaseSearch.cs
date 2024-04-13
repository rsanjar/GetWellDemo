using GetWell.Core.Models;
using GetWell.DTO.Interfaces;

namespace GetWell.Core;

public interface IBaseSearch<T> : IBasePagination where T : class, IBaseModel, new()
{
    
}