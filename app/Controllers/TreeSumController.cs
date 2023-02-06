using Microsoft.AspNetCore.Mvc;
using Triangle.Domain;

namespace app.Controllers;

[ApiController]
[Route("[controller]")]
public class TreeSumController : ControllerBase
{
    private readonly ILogger<TreeSumController> _logger;

    public TreeSumController(ILogger<TreeSumController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "SumTree")]
    public IActionResult SumTree([FromBody] string treeDefinition)
    {
        TriangleEntity triangle = new TriangleEntity(treeDefinition);
        return Ok(triangle.MaxSum);
    }
}