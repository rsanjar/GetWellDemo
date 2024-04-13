using System;
using System.Threading.Tasks;
using GetWell.Data;
using GetWell.Data.Model.Interface;
using GetWell.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GetWell.Service.Services
{
	public abstract class AuthenticatableService<TModel, TData> : BaseService<TModel, TData>, IAuthenticatable 
		where TModel : class, DTO.Interfaces.IAccount, new()
		where TData : class, IAccount, new()
	{
		#region ctor
		
		private readonly IRepository<TData> _repository;

        protected AuthenticatableService(IRepository<TData> repository) : base(repository)
		{
			_repository = repository;
		}
		
		#endregion

		public virtual async Task<bool> IsValidUserCredentialsAsync(string username, string password)
		{
			var isValid = await _repository.Entity
				.AnyAsync(c => c.MobilePhone == username && c.Password == password && c.IsActive == true);

			return isValid;
		}

		public virtual async Task<bool> IsValidUserCredentialsAsync(string phoneNumber, int smsActivationCode)
		{
			var item = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == phoneNumber);

			if(item == null)
				return false;
			
			//if attempting to validate too often return false
			if (item.LastVerificationAttemptDate != null)
            {
                var lastLoginSeconds = (item.LastVerificationAttemptDate.Value - DateTime.UtcNow).TotalSeconds;

				if(lastLoginSeconds is < 60 and >= 0)
					return false;
			}

			bool isValid = await _repository.Entity
				.AnyAsync(c => c.MobilePhone == phoneNumber && c.SmsActivationCode == smsActivationCode);

			if (isValid)
				await SetPhoneVerified(phoneNumber);
			
			await ResetSmsActivationCode(phoneNumber);

			return isValid;
		}
		
		public async Task<int> GetSmsActivationCode(string phoneNumber)
		{
			int code = 0;
			var item = await _repository.Entity.FirstOrDefaultAsync(c => c.MobilePhone == phoneNumber);

			if (item != null)
				code = item.SmsActivationCode.GetValueOrDefault(0);
			
			return code;
		}

		public virtual async Task<int> GetAccountID(string username, string password)
		{
			var result = 0;
			var item = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username && c.Password == password);

			if (item != null)
				result = item.ID;

			return result;
		}

		public virtual async Task<int> GetAccountID(string username)
		{
			var result = 0;
			var item = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username);

			if (item != null)
				result = item.ID;

			return result;
		}

		public virtual async Task<bool> IsAnExistingUserAsync(string username)
		{
			var exists = await _repository.Entity
				.AnyAsync(c => c.MobilePhone == username);

			return exists;
		}

		public virtual async Task<bool> SetPhoneVerified(string username, bool isVerified = true)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username);

			if (query != null)
			{
				query.IsPhoneVerified = isVerified;
				query.IsActive = isVerified;
				query.SmsActivationDate = isVerified ? DateTime.UtcNow : null;
				
				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public virtual async Task<bool> SetEmailVerified(string email)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.Email == email);
		
			if (query != null)
			{
				query.IsEmailVerified = true;
				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public virtual async Task<bool> UpdatePassword(string username, string oldPassword, string newPassword)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username && c.Password == oldPassword);

			if (query != null)
			{
				query.Password = newPassword;

				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public virtual async Task<bool> ResetPassword(string username, string resetPassword)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username);

			if (query != null)
			{
				query.Password = resetPassword;

				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public virtual async Task<bool> SetActive(string username, bool isActive)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username);

			if (query != null)
			{
				query.IsActive = isActive;

				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

		public virtual async Task<bool> UpdateLastLoginDate(string username)
		{
			var query = await _repository.Entity
				.FirstOrDefaultAsync(c => c.MobilePhone == username);

			if (query != null)
			{
				query.LastLoginDate = DateTime.UtcNow;

				return await _repository.Context.SaveChangesAsync() > 0;
			}

			return false;
		}

        public virtual async Task<string> GetPassword(string username)
        {
            string result = string.Empty;

            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.MobilePhone == username);

            if (query != null)
            {
                if (query.SmsActivationDate.HasValue && (DateTime.UtcNow - query.SmsActivationDate.Value).TotalMinutes < 1200)
                    return result;

                result = query.Password;
				
				query.SmsActivationDate = DateTime.UtcNow;
                
                await _repository.Context.SaveChangesAsync();
            }

            return result;
        }

        public virtual async Task<string> GetEmail(string username)
        {
            var query = await _repository.Entity
                .FirstOrDefaultAsync(c => c.MobilePhone == username);

			return query?.Email;
        }

		private async Task ResetSmsActivationCode(string phoneNumber)
		{
			var item = await _repository.Entity.FirstOrDefaultAsync(c => c.MobilePhone == phoneNumber);
			
			if (item != null)
			{
				item.SmsActivationCode = new Random().Next(10000, 99999);
				item.LastVerificationAttemptDate = DateTime.UtcNow;
				
				await _repository.Context.SaveChangesAsync();
			}
		}
	}
}