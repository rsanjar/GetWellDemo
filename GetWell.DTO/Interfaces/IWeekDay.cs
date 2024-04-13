namespace GetWell.DTO.Interfaces;

public interface IWeekDay : IBaseModel
{
	public string Name { get; set; }
	public string NameUz { get; set; }
	public string NameEn { get; set; }
	public string ShortName { get; set; }
	public string ShortNameUz { get; set; }
	public string ShortNameEn { get; set; }
}