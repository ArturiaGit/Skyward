using Skyward.Session1.WebAPI.CommandParameter;

namespace Skyward.Session1.WebAPI.Services.StaffServices;

public interface IStaffService
{
    Task<bool> GetStaffExistsByTelephoneAsync(LoginParameter parameter);
}