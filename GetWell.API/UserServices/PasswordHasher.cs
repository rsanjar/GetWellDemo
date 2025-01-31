﻿using System;
using System.Linq;
using System.Security.Cryptography;
using GetWell.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GetWell.API.UserServices
{
	public sealed class PasswordHasher : IPasswordHasher<Patient>
	{
		private const int SaltSize = 16; // 128 bit 
		private const int KeySize = 32; // 256 bit

		public PasswordHasher(IOptions<HashingOptions> options)
		{
			Options = options.Value;
		}

		private HashingOptions Options { get; }
		
		public string HashPassword(Patient user, string password)
		{
			using (var algorithm = new Rfc2898DeriveBytes(
				password,
				SaltSize,
				Options.Iterations,
				HashAlgorithmName.SHA512))
			{
				var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
				var salt = Convert.ToBase64String(algorithm.Salt);

				return $"{Options.Iterations}.{salt}.{key}";
			}
		}

		public PasswordVerificationResult VerifyHashedPassword(Patient user, string hashedPassword, string providedPassword)
		{
			var parts = hashedPassword.Split('.', 3);

			if (parts.Length != 3)
			{
				throw new FormatException("Unexpected hash format. " + 
				                          "Should be formatted as `{iterations}.{salt}.{hash}`");
			}

			var iterations = Convert.ToInt32(parts[0]);
			var salt = Convert.FromBase64String(parts[1]);
			var key = Convert.FromBase64String(parts[2]);
			
			using (var algorithm = new Rfc2898DeriveBytes(
				providedPassword,
				salt,
				iterations,
				HashAlgorithmName.SHA512))
			{
				var keyToCheck = algorithm.GetBytes(KeySize);
				bool verified = keyToCheck.SequenceEqual(key);

				return verified ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
			}
		}
	}

	public sealed class HashingOptions
	{
		public int Iterations { get; set; } = 10000;
	}
}