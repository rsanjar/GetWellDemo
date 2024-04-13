﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GetWell.DTO.Interfaces;

namespace GetWell.Core.Models
{
public class PaginatedList<T> : List<T>, IPaginatedList<T> where T : IBaseModel
	{
        protected const int MaxPageSizeInternal = 99998; //needed for cases when page size must be set to max
		private int _pageSize;
		private int _pageNumber;

		#region ctor

		public PaginatedList(string sortOrder = "ID", int pageNumber = 1, int pageSize = 5, bool? isOrderAsc = true)
		{
			sortOrder = (sortOrder ?? nameof(IBaseModel.ID)).Trim();
			var property = typeof(T).GetProperty(sortOrder, BindingFlags.IgnoreCase |  BindingFlags.Public | BindingFlags.Instance);
			
			OrderBy = property != null ? property.Name : nameof(IBaseModel.ID);
			IsOrderAscending = isOrderAsc ?? true;
			PageSize = pageSize;
			PageNumber = pageNumber;
		}

		public PaginatedList(List<T> items, int count, 
			string sortOrder = "ID", int pageNumber = 1, int pageSize = 5, bool? isOrderAsc = true) 
			: this(sortOrder)
		{
			TotalCount = count;
			PageNumber = pageNumber;
			PageSize = pageSize;
			IsOrderAscending = isOrderAsc;
			
			AddRange(items);
		}

		public PaginatedList(List<T> items, PaginatedList<T> pagination) 
			: this(items, pagination.TotalCount, pagination.OrderBy,
				pagination.PageNumber, pagination.PageSize, pagination.IsOrderAscending)
		{
		}
		
		public PaginatedList(PaginatedList<T> pagination) 
			: this(new List<T>(), pagination)
		{
		}
		
		#endregion

		public int PageSize
		{
			get => _pageSize;
            set
            {
                if ((value <= 0 || value > 200) && value != MaxPageSizeInternal)
                    _pageSize = 5;
                else
                    _pageSize = value;
            }
        }

		public int PageNumber
		{
			get => _pageNumber;
			set => _pageNumber = value <= 0 ? 1 : value;
		}

		public int TotalCount { get; set; }

		public string OrderBy { get; set; }

		public bool? IsOrderAscending { get; set; }

		public string ThenOrderBy { get; set; }

        public bool IsThenOrderAscending { get; set; } = true;
		
		public int TotalPages => (int)Math.Ceiling(TotalCount / (PageSize * 1.00));

		public bool HasNextPage => TotalPages > PageNumber;

		public bool HasPreviousPage => PageNumber > 1;

		public bool IsFirstPage => PageNumber == 1;

		public bool IsLastPage => PageNumber == TotalPages;
		
		public List<T> ResultSet => this.ToList();
    }
}