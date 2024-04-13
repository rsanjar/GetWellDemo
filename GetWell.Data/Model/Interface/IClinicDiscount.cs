using System;

namespace GetWell.Data.Model.Interface;

public interface IClinicDiscount : IDateLoggable, IBaseModel
{
	string Body { get; set; }
	decimal DiscountPercentage { get; set; }
	DateTime? EndDate { get; set; }
	string ImageUrl { get; set; }
	bool IsActive { get; set; }
	decimal PriceBeforeDiscount { get; set; }
	ServiceClinic ServiceClinic { get; set; }
	int ServiceClinicID { get; set; }
	int SortOrder { get; set; }
	DateTime? StartDate { get; set; }
	string Title { get; set; }
}