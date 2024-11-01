using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skyward.Session1.Models;
using Skyward.Session1.Services;
using Skyward.Session1.Views;

namespace Skyward.Session1.ViewModels;

public partial class MainWindowViewModel(ILogger<MainWindowViewModel> logger,IStaffService service,IDialogService dialogService,IServiceProvider serviceProvider) : ObservableObject
{
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(LoginCommand))] private string? _telephone;
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(LoginCommand))] private string? _password;

    public event Action? OnLoginSuccess;
    
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            int roleId = await service.GetStaffExistsAsync(Telephone, Password);
            switch (roleId)
            {
                case (int)Role.WarehouseSupervis:
                    OnLoginSuccess?.Invoke();
                    break;
                default:
                    await dialogService.ShowDialogAsync("登录失败，请检查电话号码或密码!!!",serviceProvider.GetRequiredService<MainWindow>());
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "无效的电话号码或密码");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "服务器连接失败");
        }
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(Telephone) && !string.IsNullOrEmpty(Password);
    }
}