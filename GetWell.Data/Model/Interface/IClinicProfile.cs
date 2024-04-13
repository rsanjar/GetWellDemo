using System;

namespace GetWell.Data.Model.Interface
{
	public interface IClinicProfile : IBaseModel
	{
		int ClinicID { get; set; }
		string Description { get; set; }

		Clinic Clinic { get; set; }
	}
}