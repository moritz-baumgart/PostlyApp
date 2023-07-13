using CommunityToolkit.Mvvm.ComponentModel;

namespace PostlyApp.ViewModels
{
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
