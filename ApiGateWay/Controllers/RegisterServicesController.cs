using Microsoft.AspNetCore.Mvc;

namespace ApiGateWay.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterServicesController:ControllerBase
{
    [HttpPost(Name = "RegisterServices")]
    public IActionResult RegisterService(RegisterServiceModel model)
    {
        ServiceRegistry.AddService(model);
        return Ok();
    }
}