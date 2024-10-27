using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Skyward.Session1.Views;

public partial class DialogWindow : Window
{
    public DialogWindow(string message)
    {
        InitializeComponent();

        MessageTextBlock.Text = message;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}