using System;
using System.Text.Json.Serialization;
using GetWell.Core.Enums;

namespace GetWell.API.Models;

public class PatientProfileRequest
{
	public int CityID { get; set; }

    public string CityName { get; set; }

	public string PreferredLanguage { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    [JsonIgnore]
    public LanguageEnum Language
    {
        get
        {
            if(Enum.TryParse(PreferredLanguage, true, out LanguageEnum result))
                return result;

            return LanguageEnum.Ru;
        }
    }

    public DateTime? DateOfBirth { get; set; }

    public bool IsMale { get; set; }

    public string FirstName { get; set; }

	public string LastName { get; set; }
}