using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GetWell.DTO.Interfaces;

public interface IClinicDiscount : IBaseModel
{
	[Display(Name = "Фото", Prompt = "Фото")]
	string ImageUrl { get; set; }
	
	[Required]
	[Display(Name = "Заголовок", Prompt = "Заголовок")]
	string Title { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    [Display(Name = "Заголовок Uz", Prompt = "Заголовок Uz")]
    public string TitleUz { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    [Display(Name = "Заголовок En", Prompt = "Заголовок En")]
    public string TitleEn { get; set; }
	
	[Required]
	[Display(Name = "Текст", Prompt = "Текст")]
	string Body { get; set; }

	[Newtonsoft.Json.JsonIgnore]
	[JsonIgnore]
    [Display(Name = "Текст Uz", Prompt = "Текст Uz")]
    public string BodyUz { get; set; }
	
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    [Display(Name = "Текст En", Prompt = "Текст En")]
    public string BodyEn { get; set; }
	
	[Display(Name = "Процент Скидки", Prompt = "Процент Скидки")]
	decimal DiscountPercentage { get; set; }

	[Display(Name = "Цена (до % скидки)", Prompt = "Цена")]
	decimal PriceBeforeDiscount { get; set; }

    decimal PriceAfterDiscount { get; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    [Display(Name = "Процент Скидки", Prompt = "Процент Скидки")]
    int DiscountPercentageInt { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    [Display(Name = "Цена (до % скидки)", Prompt = "Цена")]
    string PriceBeforeDiscountInt { get; set; }

    string ServiceName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ServiceNameUz { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ServiceNameEn { get; set; }

    string ServiceCategoryName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ServiceCategoryNameUz { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ServiceCategoryNameEn { get; set; }
	
	[Display(Name = "Действует От", Prompt = "Действует От")]
	DateTime? StartDate { get; set; }
	
	[Display(Name = "Действует До", Prompt = "Действует До")]
	DateTime? EndDate { get; set; }
	
	[Display(Name = "Активирован", Prompt = "Активирован")]
	bool IsActive { get; set; }

	[Display(Name = "Клиника", Prompt = "Клиника")]
	int ClinicID { get; set; }
	
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
	IFormFile DiscountImage { get; set; }
	
	string ClinicName { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ClinicNameEn { get; set; }
    
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    string ClinicNameUz { get; set; }
    
    string ClinicLogoUrl { get; set; }
	
	[Display(Name = "Город", Prompt = "Город")]
	public int ClinicCityID { get; set; }

	[Display(Name = "Регион", Prompt = "Регион")]
	public int ClinicCityRegionID { get; set; }

	[Display(Name = "Услуга", Prompt = "Услуга")]
	public int ServiceClinicID { get; set; }

	public int SortOrder { get; set; }
	
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
	DateTime DateCreated { get; set; }
	
    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
	DateTime? DateUpdated { get; set; }
}