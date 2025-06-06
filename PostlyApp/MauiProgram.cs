﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PostlyApp.Services;
using PostlyApp.Services.Impl;

namespace PostlyApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register services so we can inject them elsewhere.
        DependencyService.RegisterSingleton<IJwtService>(new JwtService());
        DependencyService.RegisterSingleton<IAccountService>(new AccountService());
        DependencyService.RegisterSingleton<IContentService>(new ContentService());
        DependencyService.RegisterSingleton<ISearchService>(new SearchService());

        return builder.Build();
    }
}
