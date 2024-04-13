using System.Linq;
using GetWell.DTO.Interfaces;

namespace GetWell.DTO.Localization;

public abstract class BaseLocalizable<T> : IBaseLocalizable<T> where T : class, IBaseModel, new()
{
    public void Init(LocalizationLanguageEnum language = LocalizationLanguageEnum.Ru)
    {
        var type = typeof(T);

        if (language == LocalizationLanguageEnum.Ru)
            return;

        var properties = type.GetProperties().Where(c => c.Name.EndsWith(language.ToString(), false, null)).ToList();

        foreach (var property in properties)
        {
            var defaultPropertyName = property.Name.Remove(property.Name.Length - 2, 2);

            var val = (type.GetProperty(property.Name)?.GetValue(this) ?? type.GetProperty(defaultPropertyName)?.GetValue(this))?.ToString();

            type.GetProperty(defaultPropertyName)?.SetValue(this, val);
        }
    }
}