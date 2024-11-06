using Microsoft.AspNetCore.Mvc;
using Skyward.Session1.WebAPI.Models;
using Skyward.Session1.WebAPI.Models.DisplayDto;
using Skyward.Session1.WebAPI.Services;

namespace Skyward.Session1.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WarehousesController(IWarehouseService warehouseService, ILogger<WarehousesController> logger) : ControllerBase
{
    [HttpGet("names")]
    public async Task<ActionResult<IEnumerable<string>>> GetWarehouseNamesAsync_V1()
    {
        IEnumerable<string> warehouseNames = await warehouseService.GetWarehouseNamesAsync();
        if (!warehouseNames.Any())
        {
            logger.LogWarning("没有找到仓库信息.");
            return NotFound(new { message = "没有找到仓库信息.", code = 404 });
        }
        
        logger.LogInformation("获取仓库名称成功.");
        return Ok(warehouseNames);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<WarehouseDisplayDto>> GetWarehouseInfoAsync_V1( [FromRoute]string name)
    {
        try
        {
            if (string.IsNullOrEmpty(name))
            {
                logger.LogError("仓库名称为空.");
                return BadRequest(new { message = "仓库名称不能为空.", code = 400 });
            }

            WarehouseDisplayDto? warehouseInfo = await warehouseService.GetWarehouseInfoAsync(name);
            if (warehouseInfo is null)
            {
                logger.LogWarning($"仓库 {name} 不存在.");
                return NotFound(new { message = $"仓库 {name} 不存在.", code = 404 });
            }

            logger.LogInformation($"获取仓库 {name} 信息成功.");
            return Ok(warehouseInfo);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, "仓库名称不能为空.");
            return BadRequest(new { message = "仓库名称不能为空.", code = 400 });
        }
    }
    
    [HttpGet("{warehouseName}/zones")]
    public async Task<ActionResult<IEnumerable<string>>> GetZoneNamesAsync_V1([FromRoute]string warehouseName)
    {
        try
        {
            if (string.IsNullOrEmpty(warehouseName))
            {
                logger.LogError("仓库名称为空.");
                return BadRequest(new { message = "仓库名称不能为空.", code = 400 });
            }

            IEnumerable<string> zoneNames = await warehouseService.GetZoneNamesAsync(warehouseName);
            if (!zoneNames.Any())
            {
                logger.LogWarning($"仓库 {warehouseName} 不存在.");
                return NotFound(new { message = $"仓库 {warehouseName} 不存在.", code = 404 });
            }

            logger.LogInformation($"获取仓库 {warehouseName} 库区名称成功.");
            return Ok(zoneNames);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, "仓库名称不能为空.");
            return BadRequest(new { message = "仓库名称不能为空.", code = 400 });
        }
    }
}