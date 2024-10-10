using Framework.Core.Messaging;
using InventoryManagement.Domain.DataContract.DataContract;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController(ICommandBuss commandBuss) : ControllerBase
{

    [HttpPost]
    public IActionResult CreateStock(CreateStockCommand command)
    {
        commandBuss.Send(command);
        return Ok("200");
    }
}

