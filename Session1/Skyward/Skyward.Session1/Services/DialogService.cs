using System;
using Microsoft.Extensions.DependencyInjection;
using Skyward.Session1.Views;

namespace Skyward.Session1.Services;

public class DialogService(IServiceProvider serviceProvider) : IDialogService
{
    public void ShowDialog(string message)
    {
        var dialog = new DialogWindow(message);
        dialog.ShowDialog(serviceProvider.GetRequiredService<MainWindow>());
    }
}