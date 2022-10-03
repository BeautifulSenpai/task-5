using MAUI_TASK_5.ViewModel;
using Microsoft.Maui.LifecycleEvents;

#if WINDOWS
using WinUIEx;
#endif

namespace MAUI_TASK_5;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});


		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();


#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        window.CenterOnScreen(360,480);

                        var manager = WinUIEx.WindowManager.Get(window);
                        manager.PersistenceId = "MainWindowPersistanceId";
                        manager.MinWidth = 360;
                        manager.MinHeight = 480;
                    });
                });
            });
#endif

        return builder.Build();
	}
}
