using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyward.Session1.Services.StaffServices;

public class StaffService(Lazy<HttpClient> httpClient) : IStaffService
{
    public async Task<int> GetStaffExistsAsync(string? telephone, string? password)
    {
        if(string.IsNullOrEmpty(telephone) || string.IsNullOrEmpty(password))
            throw new ArgumentException("必须提供电话和密码.");

        HttpClient client = httpClient.Value;
        StringContent body = new(JsonSerializer.Serialize(new
        {
            Telephone = telephone,
            Password = password
        }), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("api/staff/login", body);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"登录失败: {response.ReasonPhrase}");
        
        return int.Parse(await response.Content.ReadAsStringAsync());
    }
}