using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace ApiGateWay.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(Stock stockModel)
    {


        ServiceRegistry.GetHealthyServices("Stock");



        var client = new RestClient("https://localhost:7123");
        var request = new RestRequest($"api/stock");
        request.AddBody(stockModel);
        var response = client.Post(request);
        return Ok();
    }
}



public class Stock
{
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
}

