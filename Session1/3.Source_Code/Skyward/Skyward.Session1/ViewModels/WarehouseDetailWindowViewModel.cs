using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skyward.Session1.Entities;
using Skyward.Session1.Models;
using Skyward.Session1.Services;
using Skyward.Session1.Views;

namespace Skyward.Session1.ViewModels;

public partial class WarehouseDetailWindowViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IWarehouseService _warehouseService = serviceProvider.GetRequiredService<IWarehouseService>();
    private readonly IDialogService _dialogService = serviceProvider.GetRequiredService<IDialogService>();
    private readonly IPartService _partService = serviceProvider.GetRequiredService<IPartService>();
    private readonly ILogger<WarehouseService> _logger = serviceProvider.GetRequiredService<ILogger<WarehouseService>>();
    private readonly Window _ownerWindow = serviceProvider.GetRequiredService<WarehouseDetailWindow>();
    
    [ObservableProperty] private Warehouse _warehouse = new();
    [ObservableProperty] private ObservableCollection<PartDisplayDto>? _parts = [];
    [ObservableProperty] private ObservableCollection<string>? _warehouseNames;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(SearchWarehouseAndPartByNameCommand))]
    private string? _searchWarehouseName;

    public async Task InitializeAsync() =>
        await GetWarehouseNames();
    
    /// <summary>
    /// 获取仓库名称列表
    /// </summary>
    private async Task GetWarehouseNames() 
    {
        try
        {
            WarehouseNames = await _warehouseService.GetWarehouseNamesAsync();
            _logger.LogInformation("成功获取仓库名称列表。");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "获取仓库名称列表失败，请检查网络连接是否正常！！！");
            await _dialogService.ShowDialogAsync("网络连接失败，请稍后再试。", _ownerWindow);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "获取仓库名称列表失败，未找到仓库名称。");
            await _dialogService.ShowDialogAsync("未找到仓库名称，请稍后再试。", _ownerWindow);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "获取仓库名称列表失败，请检查数据格式是否正确！！！");
            await _dialogService.ShowDialogAsync("数据解析失败，请稍后再试。", _ownerWindow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取仓库名称列表失败，请查看具体错误信息：{ex.Message}");
            await _dialogService.ShowDialogAsync("发生未知错误，请联系管理员。", _ownerWindow);
        }
    }

    /// <summary>
    /// 搜索仓库信息和零件信息
    /// </summary>
    /// <param name="warehouseName">仓库名称</param>
    [RelayCommand(CanExecute = nameof(CanSearchWarehouseInfo))]
    private async Task SearchWarehouseAndPartByName(string warehouseName) =>
        await Task.WhenAll(SearchWarehouseByName(warehouseName), SearchPartsByWarehouseName(warehouseName));
    
    
    /// <summary>
    /// 搜索仓库信息
    /// </summary>
    /// <param name="warehouseName">仓库名称</param>
    private async Task SearchWarehouseByName(string warehouseName)
    {
        if (string.IsNullOrWhiteSpace(warehouseName))
        {
            _logger.LogError("搜索仓库信息失败，仓库名称不能为空！");
            await _dialogService.ShowDialogAsync("仓库名称不能为空！", _ownerWindow);
            return;
        }

        try
        {
            Warehouse = await _warehouseService.GetWarehouseInfoByNameAsync(warehouseName);
            _logger.LogInformation($"成功获取仓库信息：{Warehouse.Name}");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "搜索仓库信息失败，请检查参数是否正确！！！");
            await _dialogService.ShowDialogAsync("参数有误，请检查后重试。", _ownerWindow);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "搜索仓库信息失败，未找到指定的仓库。");
            await _dialogService.ShowDialogAsync("未找到指定的仓库，请检查后重试。", _ownerWindow);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "搜索仓库信息失败，请检查网络连接是否正常！！！");
            await _dialogService.ShowDialogAsync("网络连接失败，请稍后再试。", _ownerWindow);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "搜索仓库信息失败，请检查数据格式是否正确！！！");
            await _dialogService.ShowDialogAsync("数据解析失败，请稍后再试。", _ownerWindow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"搜索仓库信息失败，请查看具体错误信息：{ex.Message}");
            await _dialogService.ShowDialogAsync("发生未知错误，请联系管理员。", _ownerWindow);
        }
    }

    /// <summary>
    /// 搜索仓库下的零件信息
    /// </summary>
    /// <param name="warehouseName">仓库名称</param>
    private async Task SearchPartsByWarehouseName(string warehouseName)
    {
        if (string.IsNullOrEmpty(warehouseName))
        {
            _logger.LogError("搜索零件信息失败，仓库名称不能为空！");
            await _dialogService.ShowDialogAsync("仓库名称不能为空！", _ownerWindow);
            return;
        }
        
        try
        {
            Parts = await _partService.GetPartInfoByWarehouseName(warehouseName);
            _logger.LogInformation($"成功获取仓库 {warehouseName} 下的零件信息。");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "搜索零件信息失败，请检查参数是否正确！！！");
            await _dialogService.ShowDialogAsync("参数有误，请检查后重试。", _ownerWindow);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "搜索零件信息失败，未找到指定的仓库。");
            await _dialogService.ShowDialogAsync("未找到指定仓库下的零件，请检查后重试。", _ownerWindow);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "搜索零件信息失败，请检查网络连接是否正常！！！");
            await _dialogService.ShowDialogAsync("网络连接失败，请稍后再试。", _ownerWindow);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "搜索零件信息失败，请检查数据格式是否正确！！！");
            await _dialogService.ShowDialogAsync("数据解析失败，请稍后再试。", _ownerWindow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"搜索零件信息失败，请查看具体错误信息：{ex.Message}");
            await _dialogService.ShowDialogAsync("发生未知错误，请联系管理员。", _ownerWindow);
        }
    }
    
    private bool CanSearchWarehouseInfo() =>
        !string.IsNullOrEmpty(SearchWarehouseName);
}