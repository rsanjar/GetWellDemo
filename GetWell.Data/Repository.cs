using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GetWell.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Data
{
	public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseModel, new()
	{
		#region ctor

		private readonly GetWellContext _context;
		private const string ID = nameof(IBaseModel.ID);

		public Repository(GetWellContext context)
		{
            _context = context;
		}
		
		#endregion

        public GetWellContext Context => _context;

        public DbSet<TEntity> Entity => _context.Set<TEntity>();

		public List<TEntity> GetAll(int pageNumber, int pageSize, string orderByString, 
			out int count, Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null)
		{
			var query = predicate != null ? Entity.Where(predicate) : Entity;

			if (includeTables != null)
			{
				foreach (var table in includeTables)
				{
					query = query.Include(table);
				}
			}

			if (string.IsNullOrWhiteSpace(orderByString))
			{
				orderByString = ID;
			}					
			    
			var result = query.Paginate(pageNumber, pageSize, orderByString, out count);

			return result;
		}

		public async Task<List<TEntity>> GetAllAsync(Pagination<TEntity> pagination, 
			Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null)
		{
			var query = predicate != null ? Entity.Where(predicate) : Entity;

			if (includeTables != null)
			{
				foreach (var table in includeTables)
				{
					query = query.Include(table);
				}
			}

			if (string.IsNullOrWhiteSpace(pagination.OrderBy))
			{
				pagination.OrderBy = ID;
			}					
			    
			var result = await query.PaginateAsync(pagination);

			return result;
		}

		public async Task<Tuple<List<TEntity>, int>> GetAllAsync(int pageNumber, int pageSize, 
			string orderByString, Expression<Func<TEntity, bool>> predicate = null, string[] includeTables = null)
		{
			var query = predicate != null ? Entity.Where(predicate) : Entity;

			if (includeTables != null)
			{
				foreach (var table in includeTables)
				{
					query = query.Include(table);
				}
			}

			if (string.IsNullOrWhiteSpace(orderByString))
			{
				orderByString = ID;
			}					
			    
			var result = await query.PaginateAsync(pageNumber, pageSize, orderByString);

			return result;
		}
		
		public async Task<TEntity> GetAsync(int id)
		{
			return await Entity.FirstOrDefaultAsync(c => c.ID == id);
		}

	    public async Task<TEntity> GetSingleByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
	    {
	        TEntity result = null;

            if (predicate != null)
            {
	            result = await Entity.FirstOrDefaultAsync(predicate);
            }

	        return result;
	    }

	    public async Task<CrudResponse> SaveAsync(TEntity item, Expression<Func<TEntity, bool>> findSinglePredicate)
		{
			var t = Entity;

			var query = await t.SingleOrDefaultAsync(findSinglePredicate);

			if (query == null)
			{
                if (item is IDateCreatedLoggable dateCreated)
                    dateCreated.DateCreated = DateTime.UtcNow;

                if (item is IDateUpdatedLoggable dateUpdated)
                    dateUpdated.DateUpdated = null;

				t.Attach(item);
				await t.AddAsync(item);

				await _context.SaveChangesAsync();

				return Crud.Success;
			}

			item.ID = query.ID;

			return Crud.DuplicateEntryError;
		}

		public async Task<CrudResponse> SaveAsync(TEntity item)
		{
			var t = Entity;

            if (item is IDateCreatedLoggable dateCreated) 
                dateCreated.DateCreated = DateTime.UtcNow;

            if (item is IDateUpdatedLoggable dateUpdated)
                dateUpdated.DateUpdated = null;
            
			t.Attach(item);
			await t.AddAsync(item);
			await _context.SaveChangesAsync();
			
			return Crud.Success;
		}

        public async Task<CrudResponse> SaveAllAsync(List<TEntity> list)
        {
            var t = Entity;
			
            list.ForEach(c =>
            {
				if(c is IDateCreatedLoggable dateCreated)
                    dateCreated.DateCreated = DateTime.UtcNow;

                if (c is IDateUpdatedLoggable dateUpdated)
                    dateUpdated.DateUpdated = null;
            });

            t.AttachRange(list);
            await t.AddRangeAsync(list);
            await _context.SaveChangesAsync();

            return Crud.Success;
        }

		public async Task<CrudResponse> UpdateAsync(TEntity item)
		{
			if(item.ID <= 0)
				return Crud.Error;

			var t = Entity;

			var query = await t.SingleOrDefaultAsync(c => c.ID == item.ID);

			if (query != null)
			{
                if (item is IDateUpdatedLoggable dateUpdated)
                    dateUpdated.DateUpdated = DateTime.UtcNow;

                if (item is IDateCreatedLoggable dateCreated)
                    dateCreated.DateCreated = ((IDateCreatedLoggable) query).DateCreated;

                _context.Entry(query).CurrentValues.SetValues(item);

				await _context.SaveChangesAsync();

				item.ID = query.ID;

				return Crud.Success;
			}

			return Crud.ItemNotFoundError;
		}

		public async Task<CrudResponse> SaveOrUpdateAsync(TEntity item)
		{
			return item.ID <= 0 ? await SaveAsync(item) : await UpdateAsync(item);
		}

		public async Task<CrudResponse> DeleteAsync(int id)
		{
			var t = Entity;
			var query = await t.FirstOrDefaultAsync(c => c.ID == id);

			if (query != null)
			{
				t.Remove(query);
				await _context.SaveChangesAsync();

				return Crud.Success;
			}

			return Crud.ItemNotFoundError;
		}

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
		{
			int count = await Entity.CountAsync(predicate ?? (c => true));

			return count;
		}
	}
}