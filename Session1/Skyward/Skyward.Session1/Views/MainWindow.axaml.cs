using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Skyward.Session1.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}