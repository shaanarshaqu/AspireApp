using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi2.Manager.Interface;

namespace WebApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowTestController : ControllerBase
    {
        private readonly IFlowTestManager flowTestManager;
        public FlowTestController(IFlowTestManager flowTestManager)
        {
            this.flowTestManager = flowTestManager;
        }


        [HttpGet]
        public async  Task<IActionResult> RunFlow()
        {
            try
            {
                return Ok(await flowTestManager.CalliningAuthorizedFlow());
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
