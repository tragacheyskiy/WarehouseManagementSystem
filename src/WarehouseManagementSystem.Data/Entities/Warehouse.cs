using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

[Table("warehouse")]
internal sealed class Warehouse : Entity
{
    [Column("name"), StringLength(32)]
    public string Name { get; set; } = default!;

    public List<ProductStock> Products { get; set; } = default!;
}
