using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

[Table("products_transfer")]
internal sealed class ProductsTransfer : Entity
{
    [Column("table")]
    public long Time { get; set; }

    [Column("source_warehouse_id")]
    public Guid SourceWarehouseId { get; set; }

    [Column("target_warehouse_id")]
    public Guid TargetWarehouseId { get; set; }

    public List<TransferProductStock> Products { get; set; } = default!;
}
