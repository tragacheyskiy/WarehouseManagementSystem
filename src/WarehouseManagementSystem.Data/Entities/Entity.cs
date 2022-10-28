using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagementSystem.Data.Entities;

internal abstract class Entity
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [Column("modified_at")]
    public long? ModifiedAt { get; set; }

    [Column("deleted_at")]
    public long? DeletedAt { get; set; }
}
