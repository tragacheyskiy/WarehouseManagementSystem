using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.Domain.Models;
using WarehouseManagementSystem.Domain.Services.Abstractions;
using WarehouseManagementSystem.Dtos;

namespace WarehouseManagementSystem.Controllers;

[Route("api/products")]
public sealed class ProductsController : BaseController
{
	private readonly IGetProductService _getProductsService;

	public ProductsController(IGetProductService getProductsService)
	{
		_getProductsService = getProductsService ?? throw new ArgumentNullException(nameof(getProductsService));
	}

	[HttpGet]
	public async Task<DataResult<Product>> Get()
	{
		IList<Product> result = await _getProductsService.GetAsync();
		return new DataResult<Product>(result);
	}
}
