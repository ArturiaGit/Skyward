using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Skyward.Session1.Models;

namespace Skyward.Session1.Services;

public class PartService(Lazy<HttpClient> httpClient) : IPartService
{
    public async Task<ObservableCollection<PartDisplayDto>> GetPartInfoByWarehouseName(string warehouseName)
    {
        if(string.IsNullOrEmpty(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName),$"{nameof(warehouseName)} 不能为 null 或空。");

        string encodedName = Uri.EscapeDataString(warehouseName);
        string requestUri = $"/api/v1/warehouses/{encodedName}/parts";

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
            throw new InvalidOperationException($"获取仓库 {warehouseName} 下的零件失败。");

        if (!responseMessage.IsSuccessStatusCode)
        {
            string errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException($"获取仓库 {warehouseName} 下的零件失败，状态码: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}). 内容: {errorContent}");
        }

        string message = await responseMessage.Content.ReadAsStringAsync();
        ObservableCollection<PartDisplayDto>? parts;
        try
        {
            parts = JsonSerializer.Deserialize<ObservableCollection<PartDisplayDto>>(message,new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new JsonException("无法解析零件的响应内容。", ex);
        }
        
        if(parts is null)
            throw new JsonException("解析后的零件列表为 null。");
        
        return parts;
    }

    public async Task<ObservableCollection<PartToBeCheckedDisplayDto>> GetPartToBeCheckedInfoByWarehouseName(string? warehouseName, string? zoneName)
    {
        if(string.IsNullOrEmpty(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName),$"{nameof(warehouseName)} 不能为 null 或空。");

        string uri = "/api/v1/parts";
        
        HttpClient client = httpClient.Value;
        StringContent body = new(JsonSerializer.Serialize(new
        {
            WarehouseName = warehouseName,
            ZoneName = zoneName
        }), Encoding.UTF8, "application/json");
        
        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await client.PostAsync(uri, body);
        }
        catch(HttpRequestException ex)
        {
            throw new HttpRequestException("网络连接失败或请求无法完成。", ex);
        }

        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            throw new InvalidOperationException($"获取仓库 {warehouseName} 下的待盘点零件失败。");

        if (!responseMessage.IsSuccessStatusCode)
        {
            string errorContent = await responseMessage.Content.ReadAsStringAsync();
            throw new HttpRequestException($"获取仓库 {warehouseName} 下的待盘点零件失败，状态码: {(int)responseMessage.StatusCode} ({responseMessage.ReasonPhrase}). 内容: {errorContent}");
        }
        
        string message = await responseMessage.Content.ReadAsStringAsync();
        ObservableCollection<PartToBeCheckedDisplayDto>? parts;
        try
        {
            parts = JsonSerializer.Deserialize<ObservableCollection<PartToBeCheckedDisplayDto>>(message,new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new JsonException("无法解析待盘点零件的响应内容。", ex);
        }
        
        if(parts is null)
            throw new JsonException("解析后的待盘点零件列表为 null。");
        
        return parts;
    }
}