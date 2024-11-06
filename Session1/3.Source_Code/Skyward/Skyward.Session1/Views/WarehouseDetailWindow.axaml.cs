using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Skyward.Session1.ViewModels;

namespace Skyward.Session1.Views;

public partial class WarehouseDetailWindow : Window
{
    private readonly IServiceProvider _serviceProvider;

    public WarehouseDetailWindow(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    public WarehouseDetailWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        CreateInventoryCheckingTaskWindow window =
            _serviceProvider.GetRequiredService<CreateInventoryCheckingTaskWindow>();

        window.DataContext = _serviceProvider.GetRequiredService<CreateInventoryCheckingTaskWindowViewModel>();
        window.Loaded += async (_, _) =>
        {
            await (window.DataContext as CreateInventoryCheckingTaskWindowViewModel)!.InitializeAsync();
        };
        
        window.Show();

        Close();
    }
}