namespace GetWell.Data.Model.Interface
{
	public interface IClinicTypeClinic : IBaseModel
	{
		int ClinicID { get; set; }
		int ClinicTypeID { get; set; }
		bool? IsActive { get; set; }
	}
}