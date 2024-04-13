using System;

namespace GetWell.Data.Model.Interface
{
	public interface IPatientAccount : IAccount
	{
		int PatientID { get; set; }
	}
}