using CommunityToolkit.Mvvm.ComponentModel;

namespace PostlyApp.ViewModels
{
    /// <summary>
    /// The view model for the <see cref="PostlyApp.Pages.HomePage"/>.
    /// </summary>
    internal partial class HomePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _currentTab;
        [ObservableProperty]
        private bool _tab0Visible;
        [ObservableProperty]
        private bool _tab1Visible;

        public HomePageViewModel()
        {
            OnCurrentTabChanged(0);
        }

        /// <summary>
        /// Sets the visibility properties of the tabs, when the current tab is changed.
        /// </summary>
        /// <param name="newTab">The number of the tab that is changed to.</param>
        partial void OnCurrentTabChanged(int newTab)
        {
            if (newTab == 0)
            {
                Tab0Visible = true;
                Tab1Visible = false;
            }
            else if (newTab == 1)
            {
                Tab0Visible = false;
                Tab1Visible = true;
            }
        }
    }
}
