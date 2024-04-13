using System;
using System.Reflection;

namespace GetWell.Data
{
	public class Pagination<T> where T : IBaseModel
	{
		private int _pageSize;

		public Pagination(string sortOrder = "ID")
		{
			sortOrder = (sortOrder ?? nameof(IBaseModel.ID)).Trim();
			var property = typeof(T).GetProperty(sortOrder, BindingFlags.IgnoreCase |  BindingFlags.Public | BindingFlags.Instance);
			
			OrderBy = property != null ? property.Name : nameof(IBaseModel.ID);

			if(PageNumber <= 0)
				PageNumber = 1;

			IsOrderAscending ??= true;

			if (PageSize == 0)
			{
				PageSize = 5;
			}
		}
		
		public int PageSize
		{
			get => _pageSize;
			set
			{
				if (value <= 0 || value > 200)
					_pageSize = 10;
				else
					_pageSize = value;
			}
		}

		public int PageNumber { get; set; }

		public int TotalCount { get; set; }

		public string OrderBy { get; internal set; }
		
		public bool? IsOrderAscending { get; set; }

		public int TotalPages => (int)Math.Ceiling(TotalCount / (PageSize * 1.00));

		public bool HasNextPage => TotalPages > PageNumber;

		public bool HasPreviousPage => PageNumber > 1;

		public bool IsFirstPage => PageNumber == 1;

		public bool IsLastPage => PageNumber == TotalPages;
	}
}