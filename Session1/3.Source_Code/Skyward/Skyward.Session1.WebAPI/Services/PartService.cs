using Microsoft.EntityFrameworkCore;
using Skyward.Session1.WebAPI.Context;
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
}