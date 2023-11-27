using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bankoki_client_server_.Shared;

namespace Bankoki_client_server_.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO login)
        {
            SesionDTO sesionDTO = new SesionDTO();

            if (login.Password == true && login.Email == true) 
            {
                sesionDTO.LoggedIn = true;
            }
            else
            {
                sesionDTO.LoggedIn = false;
            }

            return StatusCode(StatusCodes.Status200OK, sesionDTO);
        }
    }
}
