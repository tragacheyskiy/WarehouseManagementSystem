namespace WarehouseManagementSystem.Domain.Models;

public sealed class Product
{
	public Product(Guid id, string name)
	{
		Id = id;
		Name = name ?? throw new ArgumentNullException(nameof(name));
	}

	public Guid Id { get; }
	public string Name { get; }
}
