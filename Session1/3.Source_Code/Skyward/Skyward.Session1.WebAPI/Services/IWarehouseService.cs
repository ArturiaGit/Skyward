using Skyward.Session1.WebAPI.Models;
using Skyward.Session1.WebAPI.Models.DisplayDto;

namespace Skyward.Session1.WebAPI.Services;

public interface IWarehouseService
{
    Task<IEnumerable<string>> GetWarehouseNamesAsync();
    Task<WarehouseDisplayDto?> GetWarehouseInfoAsync(string name);
}