using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Models.DTOs;

namespace PostlyApp.ViewModels
{
    internal partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserProfileViewModel? _userProfile;
        [ObservableProperty]
        private bool _followBtnVisible;
        [ObservableProperty]
        private string _followBtnText;

        partial void OnUserProfileChanged(UserProfileViewModel? userProfile)
        {
            if (userProfile != null && userProfile.Follow != null)
            {
                FollowBtnText = (bool)userProfile.Follow ? "Unfollow" : "Follow";
            }
        }
    }
}
