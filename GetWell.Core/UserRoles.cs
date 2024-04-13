namespace GetWell.Core
{
	public static class UserRoles
	{
		public const string Admin = nameof(Admin);
		public const string Clinic = nameof(Clinic);
		public const string Doctor = nameof(Doctor);
		public const string Patient = nameof(Patient);
		public const string AdminOrClinic = nameof(Admin) + "," + nameof(Clinic);
        public const string ClinicOrDoctor = nameof(Clinic) + "," + nameof(Doctor);
		public const string AdminOrClinicOrDoctor = nameof(Admin) + "," + nameof(Clinic) + "," + nameof(Doctor);
    }
}