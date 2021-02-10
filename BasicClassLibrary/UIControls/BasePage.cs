using BaseClassesLibrary.ViewModels;
using System;
using System.Windows.Controls;

namespace BaseClassesLibrary.UIControls
{
    /// <summary>
    /// The base page for all UserControl to gain base functionality
    /// </summary>
    public class BasePage<VM> : Page where VM : BaseViewModel, new()
    {
        #region Private Member
        /// <summary>
        /// The View Model associated with this page
        /// </summary>
        private VM mViewModel;
        #endregion

        #region Public Properties

        /// <summary>
        /// The view model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get => mViewModel;
            set
            {
                // If nothing has changed, return
                if (mViewModel == value)
                    return;

                // Update the value
                mViewModel = value;

                // Fire the view model changed method
                OnViewModelChanged();

                // Set the data context for this page
                DataContext = mViewModel;
            }
        }

        public event Action ViewModelChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use, if any</param>
        public BasePage(VM specificViewModel = null)
        {
            // Set specific view model
            if (specificViewModel != null)
                ViewModel = specificViewModel;
            else
            {
                ViewModel = new VM();
            }
        }

        #endregion

        /// <summary>
        /// Fired when the view model changes
        /// </summary>
        protected virtual void OnViewModelChanged()
        {
            ViewModelChanged?.Invoke();
        }
    }
}
