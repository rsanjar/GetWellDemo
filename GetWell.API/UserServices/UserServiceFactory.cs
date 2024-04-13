using System;
using GetWell.Core;
using GetWell.Service.Interface;

namespace GetWell.API.UserServices
{
	public class UserServiceFactory
	{
		private readonly IServiceProvider _serviceProvider;

		public UserServiceFactory(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public IAuthenticatable Create(string role = UserRoles.Patient)
		{
			var type = role switch
			{
				UserRoles.Admin => typeof(IAdminAccountService),
				UserRoles.Clinic => typeof(IClinicAccountService),
				UserRoles.Doctor => typeof(IClinicDoctorAccountService),
				_ => typeof(IPatientAccountService)
			};

			return (IAuthenticatable)_serviceProvider.GetService(type);
		}
	}
}