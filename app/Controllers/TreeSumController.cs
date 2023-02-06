using Microsoft.AspNetCore.Mvc;
using Triangle.Domain;
using Triangle.Infra;

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
    public IActionResult SumTree([FromBody] string treeDefinition, [FromQuery] int depthSearch = 5)
    {
        TriangleEntity triangle = new TriangleEntity(treeDefinition, depthSearch);
        return Ok(triangle.MaxSum);
    }
}