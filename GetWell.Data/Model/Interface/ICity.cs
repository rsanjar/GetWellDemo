namespace GetWell.Data.Model.Interface
{
	public interface ICity : IBaseModel
	{
		string Name { get; set; }
		int SortOrder { get; set; }
		int CountryID { get; set; }
		string NameUz { get; set; }
		string NameEn { get; set; }
	}
}