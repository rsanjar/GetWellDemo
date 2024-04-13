namespace GetWell.Data.Model.Interface;

public interface IPatientFavoriteDoctor : IDateLoggable, IBaseModel
{
	public int PatientID { get; set; }
	
	public int ClinicDoctorID { get; set; }
	
	public int SortOrder { get; set; }
	
	public ClinicDoctor ClinicDoctor { get; set; }
	
	public Patient Patient { get; set; }
}