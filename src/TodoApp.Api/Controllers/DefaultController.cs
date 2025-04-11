using Microsoft.AspNetCore.Mvc;
namespace Catalog.Api.Controllers;

[ApiController]
[Route("/")]
public class DefaultController : ControllerBase
{   
    
    [HttpGet]
    public string Get()
    {
        return "Catalog API is running";
    }
}