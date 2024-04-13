using System;
using System.Threading.Tasks;
using GetWell.DTO;
using GetWell.DTO.Localization;
using GetWell.Service.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace GetWell.API;

public class PatientCacheHelper
{
    #region ctor

    private readonly IMemoryCache _cache;
    private readonly IPatientService _patientService;
    private readonly TimeSpan _cacheTimeSpan;

    public PatientCacheHelper(IMemoryCache cache, 
        IPatientService patientService)
    {
        _cache = cache;
        _patientService = patientService;
        _cacheTimeSpan = TimeSpan.FromHours(24);
    }

    #endregion

    public async Task<LocalizationLanguageEnum> GetLanguage(int patientID)
    {
        if(patientID <= 0) 
            return LocalizationLanguageEnum.Ru;

        string key = $"{patientID}_{PatientCacheKeys.Language}";

        var language = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

            var profile = await _patientService.GetProfile(patientID);
            
            var language = (LocalizationLanguageEnum)profile.PreferredLanguage;

            return language;
        });

        return language;
    }

    public async Task<int> GetCityID(int patientID)
    {
        if(patientID <= 0) 
            return 1;

        string key = $"{patientID}_{PatientCacheKeys.CityID}";

        var cityID = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

            var profile = await _patientService.GetProfile(patientID);

            var cityID = profile.CityID ?? 1;

            return cityID;
        });

        return cityID;
    }

    public async Task<Patient> GetProfile(int patientID)
    {
        if(patientID <= 0) 
            return new Patient();
        
        string key = $"{patientID}_{PatientCacheKeys.Profile}";

        var profile = await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheTimeSpan;

            Patient profile = await _patientService.GetProfile(patientID);

            return profile;
        });

        return profile;
    }

    public void Reset(int patientID)
    {
        foreach (var i in Enum.GetValues<PatientCacheKeys>())
        {
            _cache.Remove($"{patientID}_{i}");
        }
    }
}