using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Skyward.Session1.Models;

namespace Skyward.Session1.Services;

public interface IPartService
{
    Task<ObservableCollection<PartDisplayDto>> GetPartInfoByWarehouseName(string warehouseName);
}