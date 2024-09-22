using Handle_Sharepoint.Data.Models;
using Handle_Sharepoint.Manager.Inteface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Handle_Sharepoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        public UserController(IUserManager userManager) 
        { 
            this.userManager = userManager;
        }



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var res = await userManager.GetAllUsers();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("add-user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddData([FromBody] User user)
        {
            try
            {
                return await userManager.AddUser(user) ? Ok(user) : BadRequest(user);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
