using GetWell.DTO.Interfaces;

namespace GetWell.DTO.Localization;

public interface IBaseLocalizable<T> where T : class, IBaseModel, new()
{
	void Init(LocalizationLanguageEnum language = LocalizationLanguageEnum.Ru);
}