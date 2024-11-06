using Microsoft.EntityFrameworkCore;
using Skyward.Session1.WebAPI.Context;
using Skyward.Session1.WebAPI.Entities;
using Skyward.Session1.WebAPI.Models.DisplayDto;

namespace Skyward.Session1.WebAPI.Services;

public class PartService(MyDbContext dbContext) : IPartService
{
    public async Task<IEnumerable<PartDisplayDto>> GetPartInfoByWarehouseName(string warehouseName)
    {
        if(string.IsNullOrEmpty(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName),"仓库名称不能为空");

        IEnumerable<PartDisplayDto> parts = await dbContext.Warehouses
            .TagWith("根据仓库名称获取PartInfo")
            .Where(r => r.Name == warehouseName)
            .SelectMany(r => r.Zones)
            .SelectMany(z => z.PartStorages)
            .Select(x => new PartDisplayDto
            {
                ZoneName = x.Zone.Name,
                PartName = x.Part.Name,
                Amount = x.Amount,
                Category = x.Part.Category.Name,
                MinInventory = x.Part.MinInventory,
                Specification = x.Part.Specification
            }).OrderBy(r=>r.ZoneName).ToArrayAsync();
        
        if(!parts.Any())
            throw new InvalidOperationException("未找到任何零件信息");

        return parts;
    }

    public async Task<IEnumerable<PartToBeCheckedDisplayDto>> GetPartsToBeCheckedByWarehouseName(string warehouseName,
        string? zoneName = null)
    {
        if (string.IsNullOrEmpty(warehouseName))
            throw new ArgumentNullException(nameof(warehouseName), "仓库名称不能为空");
        
        if(!dbContext.Warehouses.Any(r => r.Name.Equals(warehouseName)))
            throw new InvalidOperationException("未找到仓库信息");
        
        if(!string.IsNullOrEmpty(zoneName)&&!dbContext.Zones.Any(r=>r.Name.Equals(zoneName)))
            throw new InvalidOperationException("未找到库区信息");

        int? warehouseId = await dbContext.Warehouses
            .TagWith("根据仓库名称获取仓库ID")
            .Where(r => r.Name == warehouseName)
            .Select(r => r.Id)
            .SingleOrDefaultAsync();
        if (warehouseId is null)
            throw new InvalidOperationException("未找到仓库信息");

        IReadOnlyCollection<Zone> zones = await dbContext.Zones
            .TagWith("根据仓库ID和仓库名称（如果有）获取Zone信息")
            .Where(r => r.WarehouseId == warehouseId && (string.IsNullOrEmpty(zoneName) || r.Name == zoneName))
            .ToArrayAsync();
        if (!zones.Any())
            throw new InvalidOperationException("未找到库区信息");

        IReadOnlyCollection<int> partIds = dbContext.PartStorages
            .TagWith("根据ZoneId获取零件Id")
            .Where(r => zones.Select(x => x.Id).Contains(r.ZoneId))
            .Select(r => r.PartId)
            .ToArray();
        if (!partIds.Any())
            throw new InvalidOperationException("未找到零件信息");

        IReadOnlyCollection<int> taskId = await dbContext.InventoryCheckingTasks
            .TagWith("根据仓库Id获取任务Id")
            .Where(r => r.WarehouseId == warehouseId)
            .Select(r => r.Id)
            .ToArrayAsync();

        IReadOnlyCollection<int> taskDetailIds = await dbContext.TaskDetails
            .TagWith("根据任务Id获取任务明细Id")
            .Where(r => taskId.Contains(r.TaskId))
            .Where(r=>partIds.Contains(r.PartId))
            .Select(r => r.Id)
            .ToArrayAsync();

        IReadOnlyCollection<PartToBeCheckedDisplayDto> partToBeChecked = await dbContext.Parts
            .Where(r => partIds.Contains(r.Id))
            .Join(dbContext.TaskDetails, r => r.Id, t => t.PartId, (r, t) => new { r, t })
            .Where(r => taskDetailIds.Contains(r.t.Id))
            .Select(r => new PartToBeCheckedDisplayDto()
            {
                ZoneName = dbContext.PartStorages
                    .Where(p=>p.PartId==r.r.Id)
                    .Select(p=>p.Zone)
                    .Where(p=>p.WarehouseId==warehouseId)
                    .Select(p=>p.Name)
                    .FirstOrDefault(),
                PartId = r.r.Id,
                PartName = r.r.Name,
                Unit = r.r.Unit,
                Specification = r.r.Specification,
                StockAmount = r.t.StockAmount,
            }).OrderBy(r=>r.ZoneName)
            .ToArrayAsync();
            

        if (!partToBeChecked.Any())
            throw new InvalidOperationException("未找到任何盘点任务");

        return partToBeChecked;
    }
}