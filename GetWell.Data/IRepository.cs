using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GetWell.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data
{
	public interface IRepository<TEntity> where TEntity : class, IBaseModel, new()
    {
        public GetWellContext Context { get; }

        DbSet<TEntity> Entity { get; }

        List<TEntity> GetAll(int pageNumber, int pageSize, string orderByString, out int count, 
			Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null);
		
		Task<List<TEntity>> GetAllAsync(Pagination<TEntity> pagination, 
			Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null);
		
		Task<Tuple<List<TEntity>, int>> GetAllAsync(int pageNumber, int pageSize, string orderByString, 
			Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null);

		/// <summary>
		/// Finds an entity by ID
		/// </summary>
		/// <param name="id">ID of an entity to be found</param>
		/// <returns>Returns an entity if found, otherwise null</returns>
		Task<TEntity> GetAsync(int id);

		Task<TEntity> GetSingleByPredicateAsync(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Only inserts a new entity into the database by checking if an entity already exists by the provided predicate.
		/// </summary>
		/// <param name="item">An Entity to be inserted.</param>
		/// <param name="findSinglePredicate">Predicate to check existing entity. Predicate must return a single entity to check against the existing item.</param>
		Task<CrudResponse> SaveAsync(TEntity item, Expression<Func<TEntity, bool>> findSinglePredicate);

		/// <summary>
		/// Only inserts a new entity into the database without validation.
		/// </summary>
		/// <param name="item">An Entity to be inserted.</param>
		Task<CrudResponse> SaveAsync(TEntity item);

		/// <summary>
		/// Saves a list of objects
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
        Task<CrudResponse> SaveAllAsync(List<TEntity> list);

		/// <summary>
		/// Only updates the passed element checking by ID. If ID is 0 returned an Error.
		/// </summary>
		/// <param name="item">An Entity to be updated.</param>
		Task<CrudResponse> UpdateAsync(TEntity item);

		/// <summary>
		/// Inserts or Updates an element based on whether ID is provided or not.
		/// </summary>
		/// <param name="item">An Entity to be inserted or updated.</param>
		Task<CrudResponse> SaveOrUpdateAsync(TEntity item);

		/// <summary>
		/// Deletes an entity based on ID.
		/// </summary>
		/// <param name="id">ID of an entity to be deleted.</param>
		/// <returns>Returns success message if an entity is found and deleted.</returns>
		Task<CrudResponse> DeleteAsync(int id);

		/// <summary>
		/// Counts elements in an entity. Count can be narrowed down by a predicate.
		/// </summary>
		/// <param name="predicate">Predicate to narrow down entity elements to be counted</param>
		/// <returns>Returns number of elements in an entity.</returns>
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
	}
}