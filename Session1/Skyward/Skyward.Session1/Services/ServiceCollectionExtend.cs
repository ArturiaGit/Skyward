using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using Skyward.Session1.Services.StaffServices;
using Skyward.Session1.ViewModels;
using Skyward.Session1.Views;

namespace Skyward.Session1.Services;

public static class ServiceCollectionExtend
{
    public static void AddCommandServices(this IServiceCollection services)
    {
        services.AddLogging(builder => builder.AddNLog());

        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IDialogService, DialogService>();
        
        services.AddTransient<MainWindowViewModel>();
        services.AddSingleton<MainWindow>();
    }
}