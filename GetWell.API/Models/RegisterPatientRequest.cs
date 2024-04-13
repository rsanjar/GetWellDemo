using System.ComponentModel.DataAnnotations;
using GetWell.Core.Enums;
using GetWell.DTO;

namespace GetWell.API.Models;

public class RegisterPatientRequest
{
    public RegisterPatientRequest()
    {
        PreferredLanguage = (int)LanguageEnum.Ru;
    }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Phone { get; set; }

    public int PreferredLanguage { get; set; }


    public static explicit operator Patient(RegisterPatientRequest request)
    {
        return new Patient()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Phone = request.Phone,
            PreferredLanguage = request.PreferredLanguage,
            PatientAccount = new PatientAccount()
            {
                MobilePhone = request.Phone
            }
        };
    }
}