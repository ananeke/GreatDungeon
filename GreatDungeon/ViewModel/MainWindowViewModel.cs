using BaseClassesLibrary.Commands;
using BaseClassesLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatDungeon.ViewModel
{
    public class MainWindowViewModel: BaseViewModel
    {
        private SettingsFlyoutViewModel rightFlyoutViewModel;
        public SettingsFlyoutViewModel SettingsFlyoutViewModel
        {
            get => rightFlyoutViewModel;
            set => SetProperty(ref rightFlyoutViewModel, value);
        }
        private DelegateCommand<string> flyoutToggleCommand;
        public DelegateCommand<string> FlyoutToggleCommand
        {
            get => flyoutToggleCommand;
            set => SetProperty(ref flyoutToggleCommand, value);
        }

        public MainWindowViewModel()
        {
            FlyoutToggleCommand = new DelegateCommand<string>(FlyoutToggle);
            SettingsFlyoutViewModel = new SettingsFlyoutViewModel();
        }

        private void FlyoutToggle(string flyoutFlag)
        {
            if (flyoutFlag == "R")
                SettingsFlyoutViewModel.IsOpen = !SettingsFlyoutViewModel.IsOpen;

        }
    }
}
