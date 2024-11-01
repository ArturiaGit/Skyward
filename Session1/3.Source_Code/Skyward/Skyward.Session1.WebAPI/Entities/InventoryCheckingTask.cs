namespace Skyward.Session1.WebAPI.Entities;

public partial class InventoryCheckingTask
{
    public int Id { get; set; }

    public string TaskName { get; set; } = null!;

    public int WarehouseId { get; set; }

    public int TaskTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? FinishDate { get; set; }

    public DateTime CreateTime { get; set; }

    public int StatusId { get; set; }

    public virtual TaskStatu Statu { get; set; } = null!;

    public virtual ICollection<TaskDetail> TaskDetails { get; set; } = new List<TaskDetail>();

    public virtual TaskType TaskType { get; set; } = null!;
}
