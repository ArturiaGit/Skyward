using System.Threading.Tasks;

namespace Skyward.Session1.Services.StaffServices;

public interface IStaffService
{
    Task<bool> GetStaffExistsAsync(string? telephone, string? password);
}