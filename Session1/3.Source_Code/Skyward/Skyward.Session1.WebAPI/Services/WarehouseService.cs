using Microsoft.EntityFrameworkCore;
using Skyward.Session1.WebAPI.Context;
using Skyward.Session1.WebAPI.Models.DisplayDto;

namespace Skyward.Session1.WebAPI.Services;

public class WarehouseService(MyDbContext dbContext) : IWarehouseService
{
    /// <summary>
    /// 获取所有仓库名称
    /// </summary>
    /// <returns> 仓库名称列表 </returns>
    public async Task<IEnumerable<string>> GetWarehouseNamesAsync() =>
        await dbContext.Warehouses
            .TagWith("获取仓库名称")
            .Select(r => r.Name)
            .OrderBy(r=>r)
            .ToArrayAsync();

    /// <summary>
    /// 根据名称获取仓库信息
    /// </summary>
    /// <param name="name"> 仓库名称 </param>
    /// <returns> 仓库信息 </returns>
    /// <exception cref="ArgumentNullException"> 仓库名称不能为空 </exception>
    /// <exception cref="ArgumentException"> 仓库不存在 </exception>
    public async Task<WarehouseDisplayDto?> GetWarehouseInfoAsync(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name),"仓库名称不能为空");

        WarehouseDisplayDto? warehouse = await dbContext.Warehouses
            .TagWith("获取仓库信息")
            .Where(r => r.Name == name)
            .Select( r => new WarehouseDisplayDto
            {
                Name = r.Name,
                Area = r.Area,
                WarehouseKeeper = $"{r.ManagerNavigation.FirstName} {r.ManagerNavigation.LastName}",
                Phone = r.Phone,
                Address = r.Address,
                LastCheckingDate = dbContext.InventoryCheckingTasks
                    .Where(z=>z.WarehouseId == r.Id)
                    .Select(z=>z.FinishDate)
                    .OrderByDescending(z=>z)
                    .FirstOrDefault()
            })
            .SingleOrDefaultAsync();

        return warehouse;
    }

    /// <summary>
    /// 获取库区名称
    /// </summary>
    /// <param name="warehouseName"> 仓库名称 </param>
    /// <returns> 库区名称 </returns>
    /// <exception cref="ArgumentNullException"> 仓库名称不能为空 </exception>
    public async Task<IEnumerable<string>> GetZoneNamesAsync(string warehouseName)
    {
        if(string.IsNullOrEmpty(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName),"仓库名称不能为空");

        IEnumerable<string>? zoneNames = await dbContext.Warehouses
            .TagWith("获取库区名称")
            .Where(r => r.Name == warehouseName)
            .SelectMany(r => r.Zones)
            .Select(r => r.Name)
            .OrderBy(r => r)
            .ToArrayAsync();
        
        return zoneNames;
    }
}