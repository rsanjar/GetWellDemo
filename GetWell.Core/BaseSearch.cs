using GetWell.Core.ViewModels;
using GetWell.DTO.Interfaces;

namespace GetWell.Core;

public abstract class BaseSearch<T> : PaginationModel<T>, IBaseSearch<T> where T : class, IBaseModel, new()
{
    
}