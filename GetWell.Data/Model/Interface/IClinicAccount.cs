using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicAccount : IAccount
	{
		int ClinicID { get; set; }

		Clinic Clinic { get; set; }
	}
}