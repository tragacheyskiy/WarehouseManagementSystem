using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

[Table("product")]
internal sealed class Product : Entity
{
    [Column("name")]
    public string Name { get; set; } = default!;
}
