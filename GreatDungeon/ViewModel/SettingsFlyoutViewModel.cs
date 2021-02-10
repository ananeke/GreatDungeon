using BaseClassesLibrary.ViewModels;
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
        public SettingsFlyoutViewModel()
        {
            IsOpen = false;
        }
    }
}
