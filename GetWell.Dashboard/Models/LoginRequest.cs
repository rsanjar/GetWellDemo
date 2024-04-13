using System.ComponentModel.DataAnnotations;
using GetWell.Core;

namespace GetWell.Dashboard.Models
{
	public class LoginRequest
	{
		public LoginRequest()
		{
			Role = UserRoles.Clinic; //default role
		}

		[Required(ErrorMessage = "Введите номер телефона")]
		[Display(Name = "Номер Телефона (пр. 998971202540)")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Введите пароль")]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Выберите тип аккаунта")]
		public string Role { get; set; }

		public int AccountID { get; set; }
	}
}