using System.ComponentModel.DataAnnotations;

namespace GetWell.Dashboard.Models;

public class ChangePasswordRequest
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль", Prompt = "Пароль")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Новый Пароль", Prompt = "Новый Пароль")]
    public string NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [Display(Name = "Подтвердить Новый Пароль", Prompt = "Подтвердить Новый Пароль")]
    public string ConfirmNewPassword { get; set; }
}