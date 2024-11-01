using Microsoft.AspNetCore.Mvc;
using Skyward.Session1.WebAPI.Models.DisplayDto;
using Skyward.Session1.WebAPI.Services;

namespace Skyward.Session1.WebAPI.Controllers;

[ApiController]
[Route("api")]
public class PartController(IPartService partService,ILogger<PartController> logger) : ControllerBase
{
    [HttpGet("v1/warehouses/{warehouseName}/parts")]
    public async Task<ActionResult<IEnumerable<PartDisplayDto>>> GetPartInfoByWarehouseName_V1([FromRoute]string warehouseName)
    {
        try
        {
            IEnumerable<PartDisplayDto> parts = await partService.GetPartInfoByWarehouseName(warehouseName);
            logger.LogInformation($"获取仓库 {warehouseName} 的零件信息成功");
            
            return Ok(parts);
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, $"请求参数为空");
            return BadRequest(new {message = $"请求参数不能为空",status = StatusCodes.Status400BadRequest});
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "未找到任何零件信息");
            return NotFound(new {message = "未找到任何零件信息",status = StatusCodes.Status404NotFound});
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "获取零件信息失败");
            return StatusCode(StatusCodes.Status500InternalServerError,new {message = $"获取零件信息失败，请联系管理员，错误信息：{ex}"});
        }
    }
}