using BaseClassesLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using GreatDungeon.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatDungeon.ViewModel
{
    public class LanguageViewModel:BaseViewModel
    {
        private ObservableCollection<CultureInfo> suppoetedLanguages;
        public ObservableCollection<CultureInfo> SupportedLanguages
        {
            get => suppoetedLanguages;
            set => SetProperty(ref suppoetedLanguages, value);
        }
        private CultureInfo selectedLanguage;
        public CultureInfo SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                SetProperty(ref selectedLanguage, value as CultureInfo);
                LanguageModel.SetLocale(selectedLanguage);
            }

        }
        public LanguageViewModel(SettingsModel settingsModel)
        {
            var languages = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.IetfLanguageTag.Equals("en-US") || x.IetfLanguageTag.Equals("pl-PL")).ToList<CultureInfo>();
            SupportedLanguages = new ObservableCollection<CultureInfo>(languages);
            SelectedLanguage = SupportedLanguages.FirstOrDefault(x => x.IetfLanguageTag.Equals(settingsModel.Language));
        }
    }
}
