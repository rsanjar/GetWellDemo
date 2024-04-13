using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services;

public class ClinicDiscountService : BaseService<ClinicDiscount, Data.Model.ClinicDiscount>, IClinicDiscountService
{
	#region ctor

	private readonly IRepository<Data.Model.ClinicDiscount> _repository;

	public ClinicDiscountService(IRepository<Data.Model.ClinicDiscount> repository) : base(repository)
	{
		_repository = repository;
	}

	#endregion

	public async Task<List<ClinicDiscount>> GetAllActive()
	{
        var query = from c in _repository.Entity
			join s in _repository.Context.ServiceClinics on c.ServiceClinicID equals s.ID
			join k in _repository.Context.Clinics on s.ClinicID equals k.ID
			join f in _repository.Context.ServiceClinics on c.ServiceClinicID equals f.ID
			join l in _repository.Context.Services on f.ServiceID equals l.ID
			join m in _repository.Context.ServiceCategories on l.ServiceCategoryID equals m.ID
			join r in _repository.Context.Regions on k.RegionID equals r.ID
			join a in _repository.Context.Cities on r.CityID equals a.ID
			where c.IsActive == true && k.IsActive == true && s.IsActive == true && 
                  c.EndDate >= DateToday && c.StartDate <= DateToday
			orderby c.SortOrder, c.DateCreated descending
			select new { c, k, a, f, l, m };

		var result = await query.Select(c => new ClinicDiscount()
        {
			ID = c.c.ID,
			ImageUrl = c.c.ImageUrl,
			Body = c.c.Body,
			BodyEn = c.c.BodyEn,
			BodyUz = c.c.BodyUz,
			Title = c.c.Title,
			TitleEn = c.c.TitleEn,
			TitleUz = c.c.TitleUz,
			DiscountPercentage = c.c.DiscountPercentage,
			PriceBeforeDiscount = c.c.PriceBeforeDiscount,
			StartDate = c.c.StartDate,
			EndDate = c.c.EndDate,
			IsActive = c.c.IsActive,
			ServiceClinicID = c.c.ServiceClinicID,
			DateCreated = c.c.DateCreated,
			DateUpdated = c.c.DateUpdated,
			SortOrder = c.c.SortOrder,
			ClinicCityID = c.a.ID,
			ClinicCityRegionID = c.k.RegionID,
			ClinicID = c.k.ID,
			ClinicName = c.k.Name,
			ClinicNameEn = c.k.NameEn,
			ClinicNameUz = c.k.NameUz,
			ClinicLogoUrl = c.k.LogoUrl,
			ServiceName = c.l.Name,
			ServiceNameUz = c.l.NameUz,
			ServiceNameEn = c.l.NameEn,
			ServiceCategoryName = c.m.Name,
			ServiceCategoryNameUz = c.m.NameUz,
			ServiceCategoryNameEn = c.m.NameEn,
			ServiceClinic = c.f.Map<Data.Model.ServiceClinic, ServiceClinic>()
        }).ToListAsync();

		return result;
	}
}