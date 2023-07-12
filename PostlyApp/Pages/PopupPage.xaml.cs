using CommunityToolkit.Maui.Views;

namespace MauiToolkitPopupSample;

public partial class PopupPage : Popup
{
	public PopupPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }

    private void entry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void entry_Completed(object sender, EventArgs e)
    {

    }
}