using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core.Models;
using GetWell.Core.ViewModels;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public class NewsService : BaseService<News, Data.Model.News>, INewsService
	{
        #region ctor

        private readonly IRepository<Data.Model.News> _repository;

        public NewsService(IRepository<Data.Model.News> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public override async Task<PaginatedList<News>> GetAllAsync(PaginatedList<News> pagination)
        {
            var query = from c in _repository.Entity
                where c.IsDisabled == false
                select c;

            var result = await query.GetPaginatedListAsync(pagination);

            return result;
        }

        public override async Task<PaginatedList<News>> GetAllAsync(int pageNumber, int pageSize, string orderByString, bool isAsc = true)
        {
            var query = from c in _repository.Entity
                where c.IsDisabled == false
                select c;

            var result = await query.GetPaginatedListAsync(new PaginatedList<News>(orderByString, pageNumber, pageSize, isAsc));

            return result;
        }

        public async Task<List<News>> GetAllAsync(NewsSearch search)
        {
            var query = from news in _repository.Context.News
                where news.IsDisabled == search.IsDisabled
                select news;

            int searchLanguage = (int)search.Language;
            
            if (!string.IsNullOrWhiteSpace(search.Title))
                query = query.Where(c => EF.Functions.Like(c.Title, $"%{search.Title}%"));

            if (searchLanguage > 0)
                query = query.Where(c => c.NewsLanguage == searchLanguage);
            
            if (search.DateCreatedFrom.HasValue)
                query = query.Where(c => c.DateCreated >= search.DateCreatedFrom.Value);

            if (search.DateCreatedTo.HasValue)
                query = query.Where(c => c.DateCreated >= search.DateCreatedTo.Value);

            var result = await query.Select(c => new News
            {
                ID = c.ID,
                DateCreated = c.DateCreated,
                DateUpdated = c.DateUpdated,
                IsDisabled = c.IsDisabled,
                IsFeatured = c.IsFeatured,
                Title = c.Title,
                NewsLanguage = c.NewsLanguage
            }).GetPaginatedListAsync(search);

            return result;
        }
	}
}