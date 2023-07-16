using CommunityToolkit.Maui.Alerts;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;
using System.Collections.ObjectModel;

namespace PostlyApp.Pages;

public partial class SearchPage : ContentPage
{
    private readonly ISearchService _search;

    public ObservableCollection<UserDTO> SearchResult { get; set; } = new ObservableCollection<UserDTO>();

    public SearchPage()
    {
        InitializeComponent();
        _search = DependencyService.Resolve<ISearchService>();
        var searchViewModel = new SearchViewModel
        {
            SearchCommand = new Command(OnSearch)
        };
        searchBar.BindingContext = searchViewModel;
        BindingContext = this;
    }

    /// <summary>
    /// Is called when the search command is executed. Fetches the search results.
    /// </summary>
    public async void OnSearch()
    {
        if (searchBar.Text.Length == 0)
        {
            var toast = Toast.Make("Please enter a search query!");
            await toast.Show();
            return;
        }
        SearchResult.Clear();
        var users = await _search.SearchUsers(searchBar.Text);
        if (users != null && users.Any())

            foreach (var user in users)
            {
                SearchResult.Add(user);
            }
    }

    /// <summary>
    /// Is called when a user inside the search result is clicked, navigates to that user's profile.
    /// </summary>
    private async void OnGoToProfile(object sender, TappedEventArgs e)
    {
        if (sender is Label label)
        {
            var username = ((UserDTO)label.BindingContext).Username;
            await Shell.Current.GoToAsync($"//Profile?Username={username}");
        }
    }

    /// <summary>
    /// Resets search bar and results when the user leaves the search page.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        searchBar.Text = "";
        SearchResult.Clear();
    }
}