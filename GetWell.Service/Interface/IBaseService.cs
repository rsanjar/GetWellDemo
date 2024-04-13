using System.Collections.Generic;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Models;
using GetWell.DTO;
using GetWell.DTO.Interfaces;

namespace GetWell.Service.Interface
{
	public interface IBaseService<TModel> where TModel : class, IBaseModel, new()
	{
		Task<PaginatedList<TModel>> GetAllAsync(PaginatedList<TModel> pagination);
		
		Task<PaginatedList<TModel>> GetAllAsync(int pageNumber, int pageSize, string orderByString, bool isAsc = true);

		/// <summary>
		/// Finds an entity by ID
		/// </summary>
		/// <param name="id">ID of an entity to be found</param>
		/// <returns>Returns an entity if found, otherwise null</returns>
		Task<TModel> GetAsync(int id);
		
		/// <summary>
		/// Only inserts a new entity into the database without validation.
		/// </summary>
		/// <param name="item">An Entity to be inserted.</param>
		Task<CrudResponse> SaveAsync(TModel item);

		/// <summary>
		/// Save a list of objects
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
        Task<CrudResponse> SaveAllAsync(List<TModel> list);

		/// <summary>
		/// Only updates the passed element checking by ID. If ID is 0 returned an Error.
		/// </summary>
		/// <param name="item">An Entity to be updated.</param>
		Task<CrudResponse> UpdateAsync(TModel item);

		/// <summary>
		/// Inserts or Updates an element based on whether ID is provided or not.
		/// </summary>
		/// <param name="item">An Entity to be inserted or updated.</param>
		Task<CrudResponse> SaveOrUpdateAsync(TModel item);

		/// <summary>
		/// Deletes an entity based on ID.
		/// </summary>
		/// <param name="id">ID of an entity to be deleted.</param>
		/// <returns>Returns success message if an entity is found and deleted.</returns>
		Task<CrudResponse> DeleteAsync(int id);
	}
}