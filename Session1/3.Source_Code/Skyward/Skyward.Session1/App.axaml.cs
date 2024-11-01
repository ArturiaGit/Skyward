using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Skyward.Session1.Services;
using Skyward.Session1.ViewModels;
using Skyward.Session1.Views;

namespace Skyward.Session1;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            BindingPlugins.DataValidators.RemoveAt(0);

            ServiceCollection collection = new();
            collection.AddCommandServices();
            
            ServiceProvider provider = collection.BuildServiceProvider();
            
            MainWindow mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = provider.GetRequiredService<MainWindowViewModel>();
            
            desktop.MainWindow = mainWindow;
        }
        base.OnFrameworkInitializationCompleted();
    }
}