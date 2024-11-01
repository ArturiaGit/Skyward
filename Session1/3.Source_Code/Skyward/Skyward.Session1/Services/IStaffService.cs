using System.Threading.Tasks;

namespace Skyward.Session1.Services;

public interface IStaffService
{
    Task<int> GetStaffExistsAsync(string? telephone, string? password);
}