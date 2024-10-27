using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Skyward.Session1.Services;
using Skyward.Session1.Services.StaffServices;

namespace Skyward.Session1.ViewModels;

public partial class MainWindowViewModel(ILogger<MainWindowViewModel> logger,IStaffService service,IDialogService dialogService) : ObservableObject
{
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(LoginCommand))] private string? _telephone;
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(LoginCommand))] private string? _password;
    
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            bool exists = await service.GetStaffExistsAsync(Telephone, Password);
            if (exists)
            {
                // TODO: 登录成功后的操作
            }
            else
            {
                dialogService.ShowDialog("登录失败，请检查电话号码或密码");
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