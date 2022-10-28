using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

[Table("product_stock")]
internal sealed class ProductStock : Entity
{
    [Column("warehouse_id")]
    public Guid WarehouseId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }
}
