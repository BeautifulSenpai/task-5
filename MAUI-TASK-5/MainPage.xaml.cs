using MAUI_TASK_5.ViewModel;

namespace MAUI_TASK_5;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
		BindingContext = mainViewModel;
    }


	
}

