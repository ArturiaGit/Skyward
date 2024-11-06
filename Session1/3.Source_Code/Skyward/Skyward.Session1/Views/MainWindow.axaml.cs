using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Skyward.Session1.ViewModels;

namespace Skyward.Session1.Views;

public partial class MainWindow : Window
{
    private readonly IServiceProvider _serviceProvider;
    
    public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        DataContext = viewModel;
        _serviceProvider = serviceProvider;

        viewModel.OnLoginSuccess += HandleLoginSuccess;
    }

    public MainWindow()
    {
        InitializeComponent();
    }

    private void HandleLoginSuccess()
    {
        var warehouseDetailWindow = _serviceProvider.GetRequiredService<WarehouseDetailWindow>();
        warehouseDetailWindow.DataContext = _serviceProvider.GetRequiredService<WarehouseDetailWindowViewModel>();

        warehouseDetailWindow.Loaded += async (_, _) =>
        {
            await (warehouseDetailWindow.DataContext as WarehouseDetailWindowViewModel)!.InitializeAsync();
        };
        
        warehouseDetailWindow.Show();
        Close();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}