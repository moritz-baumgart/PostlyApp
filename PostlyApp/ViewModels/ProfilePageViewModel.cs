using CommunityToolkit.Mvvm.ComponentModel;
using PostlyApp.Models.DTOs;
using PostlyApp.Utilities;

namespace PostlyApp.ViewModels
{
    /// <summary>
    /// The view model of the <see cref="PostlyApp.Pages.ProfilePage"/>.
    /// </summary>
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

        /// <summary>
        /// Updates the follow btn text and the profile image url when the user profile changes.
        /// </summary>
        /// <param name="userProfile">The new user profile after the change.</param>
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
