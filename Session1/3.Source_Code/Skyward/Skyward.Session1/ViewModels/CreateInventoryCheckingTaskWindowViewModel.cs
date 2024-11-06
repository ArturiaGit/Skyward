using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skyward.Session1.Models;
using Skyward.Session1.Services;
using Skyward.Session1.Views;

namespace Skyward.Session1.ViewModels;

public partial class CreateInventoryCheckingTaskWindowViewModel(IServiceProvider provider) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<string>? _warehouseNames;
    [ObservableProperty] private string? _taskName;
    [ObservableProperty] private ObservableCollection<string>? _taskTypes = ["Whole Warehouse", "Portion Warehouse"];
    [ObservableProperty] private ObservableCollection<string>? _zoneNames;
    [ObservableProperty] private ObservableCollection<PartToBeCheckedDisplayDto>? _partsToBeChecked;

    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(CreateCheckTaskCommand))] private string? _startDate;
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(CreateCheckTaskCommand))] private string? _selectTaskType;
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(CreateCheckTaskCommand))] private string? _selectWarehouseName;
    [ObservableProperty][NotifyCanExecuteChangedFor(nameof(CreateCheckTaskCommand))] private string? _selectZoneName;

    private readonly IWarehouseService _warehouseService = provider.GetRequiredService<IWarehouseService>();
    private readonly IPartService _partService = provider.GetRequiredService<IPartService>();
    
    private readonly ILogger<CreateInventoryCheckingTaskWindowViewModel> _logger =
        provider.GetRequiredService<ILogger<CreateInventoryCheckingTaskWindowViewModel>>();
    private readonly IDialogService _dialogService = provider.GetRequiredService<IDialogService>();
 
    private readonly CreateInventoryCheckingTaskWindow _ownerWindow =
        provider.GetRequiredService<CreateInventoryCheckingTaskWindow>();
    
    /// <summary>
    /// 初始化仓库名称列表
    /// </summary>
    public async Task InitializeAsync() =>
        await GetWarehouseNames();
    
    [RelayCommand(CanExecute = nameof(CanCreateTask))]
    private async Task CreateCheckTaskAsync()
    {
        try
        {
            PartsToBeChecked = await _partService.GetPartToBeCheckedInfoByWarehouseName(SelectWarehouseName, SelectZoneName);
            
            _logger.LogInformation("成功创建盘点任务。");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "创建盘点任务失败，请检查网络连接是否正常！！！");
            await _dialogService.ShowDialogAsync("网络连接失败，请稍后再试。", _ownerWindow);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "创建盘点任务失败，未找到仓库名称或库区名称。");
            await _dialogService.ShowDialogAsync("未找到仓库名称或库区名称，请稍后再试。", _ownerWindow);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "创建盘点任务失败，请检查数据格式是否正确！！！");
            await _dialogService.ShowDialogAsync("数据解析失败，请稍后再试。", _ownerWindow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"创建盘点任务失败，请查看具体错误信息：{ex.Message}");
            await _dialogService.ShowDialogAsync("发生未知错误，请联系管理员。", _ownerWindow);
        }
    }


    private bool CanCreateTask =>
        (SelectTaskType == "Whole Warehouse" || !string.IsNullOrWhiteSpace(SelectZoneName)) &&
        !string.IsNullOrWhiteSpace(SelectWarehouseName) &&
        !string.IsNullOrWhiteSpace(TaskName) &&
        !string.IsNullOrWhiteSpace(SelectTaskType) &&
        !string.IsNullOrWhiteSpace(StartDate);
    
    
    
/*--------------------私有方法，用于在类内部调用--------------------*/
    /// <summary>
    /// 获取库区名称列表
    /// </summary>
    /// <param name="warehouseName"> 仓库名称 </param>
    private async Task GetZoneNames(string warehouseName)
    {
        ZoneNames?.Clear();
        
        try
        {
            ZoneNames = await _warehouseService.GetZoneNamesAsync(warehouseName);
            _logger.LogInformation("成功获取库区名称列表。");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "获取库区名称列表失败，请检查网络连接是否正常！！！");
            await _dialogService.ShowDialogAsync("网络连接失败，请稍后再试。", _ownerWindow);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "获取库区名称列表失败，未找到库区名称。");
            await _dialogService.ShowDialogAsync("未找到库区名称，请稍后再试。", _ownerWindow);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "获取库区名称列表失败，请检查数据格式是否正确！！！");
            await _dialogService.ShowDialogAsync("数据解析失败，请稍后再试。", _ownerWindow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取库区名称列表失败，请查看具体错误信息：{ex.Message}");
            await _dialogService.ShowDialogAsync("发生未知错误，请联系管理员。", _ownerWindow);
        }
    }
    
    /// <summary>
    /// 获取仓库名称列表
    /// </summary>
    private async Task GetWarehouseNames()
    {
        WarehouseNames?.Clear();
        
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
    
    
    
/*--------------------分部类方法--------------------*/    
    
    /// <summary>
    /// 当选择任务类型时，获取库区名称列表
    /// </summary>
    /// <param name="value"></param>
    partial void OnSelectTaskTypeChanging(string? value)
    {
        switch (value)
        {
            case "Whole Warehouse":
                SelectZoneName = null;
                break;
            case "Portion Warehouse":
                if (SelectWarehouseName is not null)
                    _ = GetZoneNames(SelectWarehouseName);
                break;
        }
    }
    
    /// <summary>
    /// 当开始日期改变时，检查日期是否合法
    /// </summary>
    /// <param name="value"></param>
    partial void OnStartDateChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if (DateTime.TryParse(value, out var date))
            {
                if (date < DateTime.Now)
                {
                    _logger.LogWarning("开始日期不能早于当前日期。");
                    _dialogService.ShowDialogAsync("开始日期不能早于当前日期。", _ownerWindow);
                    StartDate = DateTime.MaxValue.ToString("yyyy/M/d");
                }
            }
            else
            {
                _logger.LogWarning("日期格式不正确，请重新输入。");
                _dialogService.ShowDialogAsync("日期格式不正确，请重新输入。", _ownerWindow);
                StartDate = DateTime.MaxValue.ToString("yyyy/M/d");
            }
        }
    }
}