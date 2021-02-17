using BaseClassesLibrary.ViewModels;
using GreatDungeon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatDungeon.ViewModel
{
    public class SettingsFlyoutViewModel: BaseViewModel
    {
        private bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }
        private LanguageViewModel languageViewModel;
        public LanguageViewModel LanguageViewModel
        {
            get => languageViewModel;
            set => SetProperty(ref languageViewModel, value);
        }
        private static SettingsModel settingsModel;
        public SettingsModel SettingsModel
        {
            get => settingsModel ?? (settingsModel = new SettingsModel());
            protected set => SetProperty(ref settingsModel, value);
        }
        public SettingsFlyoutViewModel()
        {
            IsOpen = false;
            LanguageViewModel = new LanguageViewModel(SettingsModel);
        }
    }
}
