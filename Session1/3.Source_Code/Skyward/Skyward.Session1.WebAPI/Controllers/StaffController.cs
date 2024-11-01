using Microsoft.AspNetCore.Mvc;
using Skyward.Session1.WebAPI.CommandParameter;
using Skyward.Session1.WebAPI.Services;

namespace Skyward.Session1.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(IStaffService service,ILogger<StaffController> logger) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<bool>> Login_V1([FromBody] LoginParameter parameter)
    {
        try
        {
            int result = await service.GetStaffExistsByTelephoneAsync(parameter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"登录失败，请查看错误信息：{ex.Message}");
            return StatusCode(500, "登录失败，请查看日志信息。");
        }
    }
}