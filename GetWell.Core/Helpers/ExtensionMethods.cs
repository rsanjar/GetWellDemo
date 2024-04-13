using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace GetWell.Core.Helpers
{
	public static class ExtensionMethods
	{
		public static string GetClaim(this IIdentity claimsIdentity, string claimType)
		{
			var claim = ((ClaimsIdentity)claimsIdentity).Claims.FirstOrDefault(x => x.Type == claimType);

			return (claim != null) ? claim.Value : string.Empty;
		}

		public static string Role(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = ((ClaimsIdentity)claimsPrincipal.Identity)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

			return (claim != null) ? claim.Value : string.Empty;
		}

        /// <summary>
        /// Gets ClinicID or DoctorID based on the role Clinic or Doctor
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="defaultValue">Default Clinic ID</param>
        /// <returns>Return 0 if role is not Clinic or Doctor or defaultValue in case of Clinic</returns>
        public static int ID(this ClaimsPrincipal claimsPrincipal, int defaultValue = 0)
        {
            var claim = claimsPrincipal.Identity.GetClaim(ClaimTypes.PrimarySid);

            return claim.ConvertTo(defaultValue);
        }

		/// <summary>
		/// Identifies if a user is in a role a CSL roles
		/// </summary>
		/// <param name="claimsPrincipal"></param>
		/// <param name="rolesCsl">Comma Separated List of UserRole</param>
		/// <returns>Returns true if user is in any role passed</returns>
        public static bool IsInAnyRole(this ClaimsPrincipal claimsPrincipal, string rolesCsl)
        {
            var roles = rolesCsl.Split(',').ToList();

            foreach (var i in roles)
            {
                if(claimsPrincipal.IsInRole(i))
					return true;
            }

			return false;
        }
		
		public static T ConvertTo<T>(this string input) where T : struct
		{
			return ConvertTo(input, default(T));
		}

		public static T ConvertTo<T>(this string input, params string[] replace) where T : struct
		{
			return ConvertTo(input, default(T), replace);
		}

		public static T ConvertTo<T>(this string input, T defaultValue, params string [] replace) where T : struct
		{
			try
			{
				if (string.IsNullOrWhiteSpace(input))
					return defaultValue;

				foreach (var i in replace)
				{
					input = input.Replace(i, "");
				}

				return (T)Convert.ChangeType(input.Trim(), typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		}

		public static T ConvertTo<T>(this object input, T defaultValue = default(T)) where T : struct
		{
			try
			{
				if(input == null)
					return defaultValue;

				return (T)Convert.ChangeType(input, typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		}

		public static string TrimToLower(this string input, string defaultValue = null)
		{
			if (input == null)
				return defaultValue;

			return input.ToLower().Trim();
		}

		public static string TrimCapitalize(this string input, string defaultValue = "")
		{
			if (string.IsNullOrWhiteSpace(input))
				input = defaultValue;

			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.TrimToLower(string.Empty));
		}

        public static string ToLocalDate(this DateTime input, string defaultValue = "")
        {
            return input.Year > 1900 ? input.ToString("dd-MM-yyyy") : defaultValue;
        }

        public static string ToLocalDateTime(this DateTime input, string defaultValue = "")
        {
            return input.Year > 1900 ? input.ToString("dd-MM-yyyy HH:mm") : defaultValue;
        }

        public static string ToLocalDate(this DateTime? input, string defaultValue = "")
        {
            return input.HasValue ? input.Value.ToLocalDate(defaultValue) : defaultValue;
        }

        public static string ToLocalDateTime(this DateTime? input, string defaultValue = "")
        {
            return input.HasValue ? input.Value.ToLocalDateTime(defaultValue) : defaultValue;
        }
	}
}