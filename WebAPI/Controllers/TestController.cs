using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet("loggedin"), Authorize("MustBeLoggedIn")]
    public ActionResult GetAsLoggedIn()
    {
        return Ok("This was accepted as logged in");
    }
}