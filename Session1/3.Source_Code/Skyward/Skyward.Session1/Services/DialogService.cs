using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Skyward.Session1.Views;

namespace Skyward.Session1.Services;

public class DialogService: IDialogService
{
    public async Task ShowDialogAsync(string message,Window ownerWindow)
    {
        var dialog = new DialogWindow(message);
        await dialog.ShowDialog(ownerWindow);
    }
}