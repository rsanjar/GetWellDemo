using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Data;
using GetWell.Service.Interface;
using Crud = GetWell.Core.Enums.Crud;
using CrudResponse = GetWell.Core.CrudResponse;

namespace GetWell.Service.Services
{
	public abstract class BaseService<TModel, TData> : IBaseService<TModel> 
		where TModel : class, DTO.Interfaces.IBaseModel, new() 
		where TData : class, IBaseModel, new()
	{
		#region ctor

		private readonly IRepository<TData> _repository;

		public BaseService(IRepository<TData> repository)
		{
			_repository = repository;
		}

		#endregion
		
        protected static DateTime DateToday => DateOnly.FromDateTime(DateTime.UtcNow).ToDateTime(TimeOnly.MinValue);

		public virtual async Task<PaginatedList<TModel>> GetAllAsync(PaginatedList<TModel> pagination)
        {
            return await _repository.Entity.GetPaginatedListAsync(pagination);
		}
		
		public virtual async Task<PaginatedList<TModel>> GetAllAsync(int pageNumber, int pageSize, string orderByString, bool isAsc = true)
		{
            return await _repository.Entity.GetPaginatedListAsync(new PaginatedList<TModel>(orderByString, pageNumber, pageSize, isAsc));
		}

		public virtual async Task<TModel> GetAsync(int id)
		{
            return (await _repository.GetAsync(id)).Map<TData, TModel>();
		}

		public virtual async Task<CrudResponse> SaveAsync(TModel item)
        {
            var newItem = Convert(item);
            var result = await _repository.SaveAsync(newItem);
            
            item.ID = newItem.ID;

            return Response(result);
        }

        public virtual async Task<CrudResponse> SaveAllAsync(List<TModel> list)
        {
            var newList = new List<TData>();

			list.ForEach(c => newList.Add(Convert(c)));

            var result = await _repository.SaveAllAsync(newList);
			
            return Response(result);
        }

		public virtual async Task<CrudResponse> UpdateAsync(TModel item)
		{
			return Response(await _repository.UpdateAsync(Convert(item)));
		}

		public virtual async Task<CrudResponse> SaveOrUpdateAsync(TModel item)
		{
            var newItem = Convert(item);
            var result = await _repository.SaveOrUpdateAsync(newItem);

            item.ID = newItem.ID;

            return Response(result);
        }

		public virtual async Task<CrudResponse> DeleteAsync(int id)
		{
			return Response(await _repository.DeleteAsync(id));
		}
		
        protected static async Task<TModel> FirstOrDefaultAsync(IQueryable<TData> t)
        {
            return await t.FirstOrDefaultAsync<TData, TModel>();
        }

        protected static async Task<List<TModel>> ToListAsync(IQueryable<TData> t)
        {
            return await t.ToListAsync<TData, TModel>();
        }

        private static TData Convert(TModel t)
		{
			return ObjectMapper<TModel, TData>.Mapper.Map<TData>(t);
		}
		
        private static CrudResponse Response(Data.CrudResponse response)
        {
            return new ((Crud)((int)response.MessageKey));
        }
    }
}