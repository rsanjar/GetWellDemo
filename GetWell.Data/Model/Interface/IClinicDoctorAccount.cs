
namespace GetWell.Data.Model.Interface
{
	public interface IClinicDoctorAccount : IAccount
	{
		int ClinicDoctorID { get; set; }
		
		ClinicDoctor ClinicDoctor { get; set; }
	}
}