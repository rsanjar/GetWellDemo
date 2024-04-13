using System;
using System.Reflection;
using GetWell.Core.Models;
using GetWell.DTO.Interfaces;

namespace GetWell.Core.ViewModels
{
    public class PaginationModel<T> : IBasePagination where T : IBaseModel
    {
        protected const int MaxPageSizeInternal = 99998; //needed for cases when page size must be set to max
        private int _pageSize;
        private int _pageNumber;

        public PaginationModel(string sortOrder = "ID", int pageNumber = 1, int pageSize = 5, bool? isOrderAsc = true)
        {
            sortOrder = (sortOrder ?? nameof(IBaseModel.ID)).Trim();
            var property = typeof(T).GetProperty(sortOrder,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            OrderBy = property != null ? property.Name : nameof(IBaseModel.ID);
            IsOrderAscending = isOrderAsc ?? true;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if ((value <= 0 || value > 100) && value != MaxPageSizeInternal)
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

        public int TotalPages => (int) Math.Ceiling(TotalCount / (PageSize * 1.00));

        public bool HasNextPage => TotalPages > PageNumber;

        public bool HasPreviousPage => PageNumber > 1;

        public bool IsFirstPage => PageNumber == 1;

        public bool IsLastPage => PageNumber == TotalPages;

        public static implicit operator PaginatedList<T>(PaginationModel<T> model)
        {
            return new(model.OrderBy, model.PageNumber, model.PageSize, model.IsOrderAscending);
        }
    }
}