namespace GetWell.Data.Model.Interface
{
	public interface IClinicPhone : IBaseModel
	{
		string Phone { get; set; }
		bool IsMain { get; set; }
		bool? IsMobile { get; set; }
		bool IsDisabled { get; set; }
		int SortOrder { get; set; }
		int ClinicID { get; set; }
	}
}