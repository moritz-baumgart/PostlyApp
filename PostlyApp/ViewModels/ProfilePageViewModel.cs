using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;

namespace PostlyApp.ViewModels
{
    internal partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserProfileViewModel? _userProfile;
        [ObservableProperty]
        private UriImageSource? _profilePicture;
        [ObservableProperty]
        private bool _followBtnVisible;
        [ObservableProperty]
        private string _followBtnText;

        partial void OnUserProfileChanged(UserProfileViewModel? userProfile)
        {
            if (userProfile != null)
            {
                if (userProfile.Follow != null)
                {
                    FollowBtnText = (bool)userProfile.Follow ? "Unfollow" : "Follow";
                }

                if (userProfile.ProfileImageUrl != null)
                {
                    ProfilePicture = new UriImageSource
                    {
                        Uri = new Uri(Constants.API_BASE + userProfile.ProfileImageUrl)
                    };
                } else
                {
                    ProfilePicture = null;
                }
            }
        }
    }
}
