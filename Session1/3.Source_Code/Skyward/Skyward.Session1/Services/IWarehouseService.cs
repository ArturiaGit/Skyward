using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Skyward.Session1.Entities;
using Skyward.Session1.Models;

namespace Skyward.Session1.Services;

public interface IWarehouseService
{
    Task<ObservableCollection<string>> GetWarehouseNamesAsync();
    Task<Warehouse> GetWarehouseInfoByNameAsync(string warehouseName);
}