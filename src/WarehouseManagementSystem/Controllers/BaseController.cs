using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace WarehouseManagementSystem.Controllers;

[ApiController, Produces(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
}
