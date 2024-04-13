using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using GetWell.Data;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service
{
    internal static class AutoMapperExtensions
    {
        public static IQueryable<TModel> ProjectTo<TData, TModel>(this IQueryable<TData> query)
            where TData : class, IBaseModel, new()
            where TModel : class, DTO.Interfaces.IBaseModel, new()
        {
            return query.ProjectTo<TModel>(ObjectMapper<TModel, TData>.Mapper.ConfigurationProvider);
        }

        public static TModel Map<TData, TModel>(this TData t)
            where TData : class, IBaseModel, new()
            where TModel : class, DTO.Interfaces.IBaseModel, new()
        {
            return ObjectMapper<TModel, TData>.Mapper.Map<TModel>(t);
        }

        public static async Task<TModel> FirstOrDefaultAsync<TData, TModel>(this IQueryable<TData> query,
            Expression<Func<TModel, bool>> predicate = null)
            where TData : class, IBaseModel, new()
            where TModel : class, DTO.Interfaces.IBaseModel, new()
        {
            var baseModels = query.Select(c => c.Map<TData, TModel>());

            if(predicate != null)
                return await baseModels.Where(predicate).FirstOrDefaultAsync();
            
            return await baseModels.FirstOrDefaultAsync();
        }

        public static async Task<List<TModel>> ToListAsync<TData, TModel>(this IQueryable<TData> query)
            where TData : class, IBaseModel, new()
            where TModel : class, DTO.Interfaces.IBaseModel, new()
        {
            return await query.Select(c => c.Map<TData, TModel>()).ToListAsync();
        }
    }
}