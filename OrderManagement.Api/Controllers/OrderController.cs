using Framework.Core.Messaging;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Domain.Contract.DataContract;

namespace OrderManagement.Api.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ICommandBuss commandBuss) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderCommand command)
        {
            commandBuss.Send(command);
            return Ok("200");
        }
    }

