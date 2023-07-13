using CommunityToolkit.Maui.Views;

namespace MauiToolkitPopupSample;

public partial class PostdetailView : Popup
{
	public PostdetailView()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Close();
    }
}