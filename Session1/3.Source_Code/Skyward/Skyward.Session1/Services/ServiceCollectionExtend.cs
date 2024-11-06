using System;
using System.Net.Http;
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
        services.AddSingleton(new Lazy<HttpClient>(() => new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7000"),
            Timeout = TimeSpan.FromSeconds(180)
        }));

        services.AddTransient<IStaffService, StaffService>();
        services.AddTransient<IWarehouseService, WarehouseService>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient<IPartService, PartService>();
        
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<WarehouseDetailWindowViewModel>();
        services.AddSingleton<CreateInventoryCheckingTaskWindowViewModel>();
        
        services.AddSingleton<MainWindow>(r =>
        {
            var vm = r.GetRequiredService<MainWindowViewModel>();
            return new MainWindow(vm, r);
        });
        services.AddSingleton<WarehouseDetailWindow>(r => new WarehouseDetailWindow(r));
        services.AddSingleton<CreateInventoryCheckingTaskWindow>();
    }
}