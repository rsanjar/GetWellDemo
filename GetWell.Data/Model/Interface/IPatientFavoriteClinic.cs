namespace GetWell.Data.Model.Interface;

public interface IPatientFavoriteClinic : IDateLoggable, IBaseModel
{
	public int PatientID { get; set; }
	
	public int ClinicID { get; set; }
	
	public int SortOrder { get; set; }
	
	public Clinic Clinic { get; set; }

	public Patient Patient { get; set; }
}