using System;

namespace GetWell.Data.Model.Interface
{
	public interface IAccount : IDateLoggable, IBaseModel
	{
		public string MobilePhone { get; set; }
		public string Password { get; set; }
		public bool IsActive { get; set; }
		public bool IsPhoneVerified { get; set; }
        public DateTime? LastLoginDate { get; set; }
		public Guid UniqueKey { get; set; }
		public int? SmsActivationCode { get; set; }
		public DateTime? SmsActivationDate { get; set; }
		public string Email { get; set; }
		public bool IsEmailVerified { get; set; }
		public DateTime? EmailVerificationDate { get; set; }
		public DateTime? LastVerificationAttemptDate { get; set; }
	}
}