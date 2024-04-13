namespace GetWell.Data.Model.Interface
{
	public interface IWeekDay : IBaseModel
	{
		string Name { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
		string ShortName { get; set; }
		string ShortNameUz { get; set; }
		string ShortNameEn { get; set; }
	}
}