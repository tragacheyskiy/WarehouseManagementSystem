using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

[Table("transfer_product_stock")]
internal class TransferProductStock : Entity
{
    [Column("products_transfer_id")]
    public Guid ProductsTransferId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }
}
