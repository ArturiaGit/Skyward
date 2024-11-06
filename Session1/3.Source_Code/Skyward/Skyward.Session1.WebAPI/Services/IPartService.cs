using Skyward.Session1.WebAPI.Models.DisplayDto;

namespace Skyward.Session1.WebAPI.Services;

public interface IPartService
{
    Task<IEnumerable<PartDisplayDto>> GetPartInfoByWarehouseName(string warehouseName);
    Task<IEnumerable<PartToBeCheckedDisplayDto>> GetPartsToBeCheckedByWarehouseName(string warehouseName,string? zoneName=null);
}