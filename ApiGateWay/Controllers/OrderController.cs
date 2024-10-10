using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace ApiGateWay.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderApiGateWayController : ControllerBase
{
    [HttpPost(Name = "ApiGateways.Order")]
    public IActionResult post(Order model)
    {
        ServiceRegistry.GetHealthyServices("Order");
        var client = new RestClient("https://localhost:7147");
        var request = new RestRequest("api/Order");
        request.AddBody(model);
        var response = client.Post(request);
        return Ok();

    }
}




public class Order
{
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> OrderItemDtos { get; set; } = new();
}
public class OrderItem
{
    public Guid ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public uint Quantity { get; set; }

}

