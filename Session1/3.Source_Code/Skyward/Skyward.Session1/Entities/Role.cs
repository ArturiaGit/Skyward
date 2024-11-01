namespace Skyward.Session1.Models;

/// <summary>
/// 仓库管理系统中可能角色的枚举。
/// </summary>
public enum Role
{
    AssetAdministrator=1,
    
    /// <summary>
    /// 仓库主管
    /// </summary>
    WarehouseSupervis=2,
    
    TransportationAdministrator=3,
    
    VehicleTeamAdministrator=4,
    
    /// <summary>
    /// 统计管理员
    /// </summary>
    StatisticsAdministrator=5,
    
    WarehouseKeeper=6
}