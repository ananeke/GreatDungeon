using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace GreatDungeon.Model
{
    public static class LanguageModel
    {
        public static string GetLocalizedString(string key)
        {
            LocExtension localizeExtension = new LocExtension(key);
            localizeExtension.ResolveLocalizedValue(out string uiString);
            return uiString;
        }
        public static void SetLocale(CultureInfo culture = null)
        {
            if (culture != null)
            {
                LocalizeDictionary.Instance.Culture = culture;
                CultureChange?.Invoke();
            }
        }
        public static event Action CultureChange;
    }
}
