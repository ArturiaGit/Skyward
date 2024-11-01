using Skyward.Session1.WebAPI.CommandParameter;

namespace Skyward.Session1.WebAPI.Services;

public interface IStaffService
{
    Task<int> GetStaffExistsByTelephoneAsync(LoginParameter parameter);
}