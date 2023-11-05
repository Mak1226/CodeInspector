using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cloud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult Create(
            [FromBody] AnalysisBody analysisbody)
        {
            string result = Analysis.PerformAnalysis(analysisbody);
            return Ok(result);
        }
    }
}
