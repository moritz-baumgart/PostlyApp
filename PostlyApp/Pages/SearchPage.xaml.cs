namespace PostlyApp.Pages;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
        SearchBar searchBar =
            new SearchBar { Placeholder = "Search User" };
    }
}