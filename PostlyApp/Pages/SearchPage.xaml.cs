using CommunityToolkit.Maui.Alerts;
using PostlyApp.Models.DTOs;
using PostlyApp.Services;
using PostlyApp.ViewModels;
using System.Collections.ObjectModel;

namespace PostlyApp.Pages;

public partial class SearchPage : ContentPage
{
    private readonly ISearchService _search;

    public ObservableCollection<string> SearchResult { get; set; } = new ObservableCollection<string>();

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
                if (user.DisplayName != null && user.DisplayName.Length > 0)
                {
                    SearchResult.Add($"{user.DisplayName} (@{user.Username})");
                }
                else
                {
                    SearchResult.Add($"@{user.Username}");
                }
            }
    }
}