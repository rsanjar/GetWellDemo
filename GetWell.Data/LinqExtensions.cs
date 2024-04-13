﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GetWell.Data
{
	public static class LinqExtensions
	{
		/// <summary>
		/// Sorts the IQueryable input by a provided String column name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="sortingString">
		/// Column name. Also sorting direction could be provided(ex. "LastName desc") default sorting direction is "asc".
		/// Provide empty string or null not to sort the collection.
		/// </param>
		/// <returns></returns>
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortingString)
		{
			var items = sortingString.Trim().Split(' ');
			string orderDirection = string.Empty;
			string columnName = items[0];

			if(items.Any())
				orderDirection = items[items.Length - 1];

			if(orderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase) ||
				orderDirection.Equals("descending", StringComparison.OrdinalIgnoreCase))
			{
				return ApplyOrder(source, columnName, "OrderByDescending");
			}

			return ApplyOrder(source, columnName, "OrderBy");
		}

		/// <summary>
		/// Sorts the IQueryable input by a provided String column name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="sortingString">Column name. Also sorting direction could be provided(ex. "LastName desc") default sorting direction is "asc"</param>
		/// <returns></returns>
		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string sortingString)
		{
			var items = sortingString.Trim().Split(' ');
			string orderDirection = string.Empty;
			string columnName = items[0];

			if(items.Any())
				orderDirection = items[items.Length - 1];

			if(orderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase) ||
				orderDirection.Equals("descending", StringComparison.OrdinalIgnoreCase))
			{
				return ApplyOrder(source, columnName, "ThenByDescending");
			}

			return ApplyOrder(source, columnName, "ThenBy");
		}

		/// <summary>
		/// Returns a paginated list of items
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query">IQueryable query</param>
		/// <param name="pageNumber">Starts from 1</param>
		/// <param name="pageSize">Number of items to take. If it's less than 1, then it's set to 1</param>
		/// <param name="count">Number of items in the query input</param>
		/// <returns>Returns a paginated list of items. If pageNumber is less then zero it's set to 1. </returns>
		public static List<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize, out int count)
		{
			return Paginate(query, pageNumber, pageSize, null, out count);
		}

		public static List<T> Paginate<T>(this IQueryable<T> query, Pagination<T> pagination) where T : IBaseModel
		{
			if (!pagination.IsOrderAscending.GetValueOrDefault(true) && !string.IsNullOrWhiteSpace(pagination.OrderBy))
				pagination.OrderBy = $"{pagination.OrderBy} desc";

			var result = Paginate(query, pagination.PageNumber, pagination.PageSize, pagination.OrderBy, out int count);
			pagination.TotalCount = count;

			return result;
		}

		public static async Task<List<T>> PaginateAsync<T>(this IQueryable<T> query, Pagination<T> pagination) where T : IBaseModel
		{
			if (!pagination.IsOrderAscending.GetValueOrDefault(true) && !string.IsNullOrWhiteSpace(pagination.OrderBy))
				pagination.OrderBy = $"{pagination.OrderBy} desc";

			var result = await PaginateAsync(query, pagination.PageNumber, pagination.PageSize, pagination.OrderBy);
			pagination.TotalCount = result.Item2;

			return result.Item1;
		}

		/// <summary>
		/// Returns a paginated list of items
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query">IQueryable query</param>
		/// <param name="pageNumber">Starts from 1</param>
		/// <param name="pageSize">Number of items to take. If it's less than 1, then it's set to 1</param>
		/// <param name="sortingString">
		/// Column name. Also sorting direction could be provided(ex. "LastName desc") default sorting direction is "asc".
		/// Provide empty string or null not to sort the collection.
		/// </param>
		/// <param name="count">Number of items in the query input</param>
		/// <returns>Returns a paginated list of items. If pageNumber is less then zero it's set to 1</returns>
		public static List<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize, string sortingString, out int count)
		{
			count = query.Count();

			if(!string.IsNullOrEmpty(sortingString))
				query = query.OrderBy(sortingString);

			if(pageSize < 1) //pageSize cannot be less then 1
				pageSize = 1;

			if(pageNumber < 1) //Page number cannot be less then 1
				pageNumber = 1;

			return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
		}

		public static async Task<Tuple<List<T>, int>> PaginateAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, string sortingString)
		{
			int count = await query.CountAsync();

			if(!string.IsNullOrEmpty(sortingString))
				query = query.OrderBy(sortingString);

			if(pageSize < 1) //pageSize cannot be less then 1
				pageSize = 1;

			if(pageNumber < 1) //Page number cannot be less then 1
				pageNumber = 1;

			var list = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			return Tuple.Create(list, count);
		}
		
		private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			string[] props = property.Split('.');
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "x");
			Expression expr = arg;
			foreach(string prop in props)
			{
				// use reflection (not ComponentModel) to mirror LINQ
				System.Reflection.PropertyInfo pi = type.GetProperty(prop);
				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}
			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

			object result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
					.MakeGenericMethod(typeof(T), type)
					.Invoke(null, new object[] { source, lambda });
			return (IOrderedQueryable<T>)result;
		}
	}
}