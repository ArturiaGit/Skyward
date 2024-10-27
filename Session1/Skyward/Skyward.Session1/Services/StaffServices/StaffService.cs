using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyward.Session1.Services.StaffServices;

public class StaffService : IStaffService
{
    private readonly Lazy<HttpClient> _client = new Lazy<HttpClient>(() => new HttpClient()
    {
        BaseAddress = new Uri("https://localhost:7000"),
        Timeout = TimeSpan.FromSeconds(180),
    });

    public async Task<bool> GetStaffExistsAsync(string? telephone, string? password)
    {
        if(string.IsNullOrEmpty(telephone) || string.IsNullOrEmpty(password))
            throw new ArgumentException("必须提供电话和密码.");

        HttpClient client = _client.Value;
        StringContent body = new(JsonSerializer.Serialize(new
        {
            Telephone = telephone,
            Password = password
        }), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("api/staff/login_V1", body);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"登录失败: {response.ReasonPhrase}");
        
        return bool.Parse(await response.Content.ReadAsStringAsync());
    }
}