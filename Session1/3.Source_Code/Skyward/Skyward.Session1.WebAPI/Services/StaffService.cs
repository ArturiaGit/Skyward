using Microsoft.EntityFrameworkCore;
using Skyward.Session1.WebAPI.CommandParameter;
using Skyward.Session1.WebAPI.Context;

namespace Skyward.Session1.WebAPI.Services;

public class StaffService(MyDbContext dbContext) : IStaffService
{
    /// <summary>
    /// 通过手机号和密码查询员工是否存在
    /// </summary>
    /// <param name="parameter">  登录参数 </param>
    /// <returns> 是否存在 </returns>
    /// <exception cref="ArgumentNullException"> 参数不能为空. </exception>
    public async Task<int> GetStaffExistsByTelephoneAsync(LoginParameter parameter)
        => await dbContext.Staff
            .TagWith("根据 Telephone 查询员工是否存在")
            .Where(r => r.Telephone == parameter.Telephone && r.Password == parameter.Password)
            .AsNoTracking()
            .Select(r => r.RoleId)
            .FirstOrDefaultAsync();
}