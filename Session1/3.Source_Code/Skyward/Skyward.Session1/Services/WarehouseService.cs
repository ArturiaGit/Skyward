using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Skyward.Session1.Entities;
using Skyward.Session1.Models;

namespace Skyward.Session1.Services;

public class WarehouseService(Lazy<HttpClient> httpClient) : IWarehouseService
{
    /// <summary>
    /// 获取所有仓库名称
    /// </summary>
    /// <returns> 仓库名称列表 </returns>
    /// <exception cref="HttpRequestException"> 网络连接失败 </exception>
    /// <exception cref="InvalidOperationException"> 解析仓库名称失败 </exception>
    public async Task<ObservableCollection<string>> GetWarehouseNamesAsync()
    {
        string requestUri = $"/api/v1/warehouses/names";
        HttpClient client = httpClient.Value;
        
        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await client.GetAsync(requestUri);
        }
        catch(HttpRequestException ex)
        {
            throw new HttpRequestException("网络连接失败或请求无法完成。", ex);
        }
        
        if(responseMessage.StatusCode == HttpStatusCode.NotFound)
            throw new InvalidOperationException("未找到仓库名称。");

        if (!responseMessage.IsSuccessStatusCode)
        {
            string errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException($"获取仓库名称失败，状态码: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}). 内容: {errorContent}");
        }
        
        string message = await responseMessage.Content.ReadAsStringAsync();
        ObservableCollection<string>? warehouseNames;
        try
        {
            warehouseNames = JsonSerializer.Deserialize<ObservableCollection<string>>(message);
        }
        catch (JsonException ex)
        {
            throw new JsonException("无法解析仓库名称的响应内容。", ex);
        }
        
        if (warehouseNames is null)
            throw new JsonException("解析后的仓库名称列表为 null。");

        return warehouseNames;
    }
    
    /// <summary>
    /// 获取指定仓库信息
    /// </summary>
    /// <param name="warehouseName">仓库名称</param>
    /// <returns>仓库信息</returns>
    /// <exception cref="ArgumentNullException">warehouseName 为 null 或空.</exception>
    /// <exception cref="InvalidExpressionException">找不到指定名称的仓库.</exception>
    /// <exception cref="HttpRequestException">网络连接失败或其他 HTTP 请求错误.</exception>
    /// <exception cref="JsonException">解析仓库信息失败.</exception>
    public async Task<Warehouse> GetWarehouseInfoByNameAsync(string warehouseName)
    {
        if (string.IsNullOrWhiteSpace(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName), $"{nameof(warehouseName)} 不能为 null 或空.");

        string encodedName = Uri.EscapeDataString(warehouseName);
        string requestUri = $"/api/v1/warehouses/{encodedName}";

        HttpClient client = httpClient.Value;

        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await client.GetAsync(requestUri);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("网络连接失败或请求无法完成。", ex);
        }

        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            throw new InvalidOperationException($"未找到名称为 '{warehouseName}' 的仓库。");

        if (!responseMessage.IsSuccessStatusCode)
        {
            string errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException($"获取仓库信息失败，状态码: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}). 内容: {errorContent}");
        }

        string message = await responseMessage.Content.ReadAsStringAsync();
        Warehouse? warehouse;
        try
        {
            warehouse = JsonSerializer.Deserialize<Warehouse>(message, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new JsonException("无法解析仓库信息的响应内容。", ex);
        }

        if (warehouse is null)
            throw new JsonException("解析后的仓库信息为 null。");

        return warehouse;
    }

    /// <summary>
    /// 获取指定仓库的库区名称列表
    /// </summary>
    /// <param name="warehouseName"> 仓库名称 </param>
    /// <returns> 库区列表 </returns>
    /// <exception cref="ArgumentNullException"> 参数不能为空 </exception>
    /// <exception cref="HttpRequestException"> 请求失败 </exception>
    /// <exception cref="InvalidOperationException"> 找不到指定仓库的库区 </exception>
    /// <exception cref="JsonException"> 解析失败 </exception>
    public async Task<ObservableCollection<string>> GetZoneNamesAsync(string warehouseName)
    {
        if(string.IsNullOrWhiteSpace(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName), $"{nameof(warehouseName)} 不能为 null 或空.");
        
        string encodedName = Uri.EscapeDataString(warehouseName);
        string requestUri = $"api/v1/warehouses/{encodedName}/zones";

        HttpClient client = httpClient.Value;
        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await client.GetAsync(requestUri);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("网络连接失败或请求无法完成。", ex);
        }
        
        if(responseMessage.StatusCode == HttpStatusCode.NotFound)
            throw new InvalidOperationException($"未找到名称为 '{warehouseName}' 的库区。");

        if (!responseMessage.IsSuccessStatusCode)
        {
            string errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException($"获取库区名称失败，状态码: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}). 内容: {errorContent}");
        }
        
        string message = await responseMessage.Content.ReadAsStringAsync();
        ObservableCollection<string>? zoneNames;
        try
        {
            zoneNames = JsonSerializer.Deserialize<ObservableCollection<string>>(message);
        }
        catch (JsonException ex)
        {
            throw new JsonException("无法解析库区名称的响应内容。", ex);
        }
        
        if(zoneNames is null)
            throw new JsonException("解析后的库区名称列表为 null。");

        return zoneNames;
    }
}