using NTierLoginJWT.BLL.Services;
using NTierLoginJWT.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using NTierLoginJWT.DAL.Context;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private CRUDContext _context;
        public UsersController(IConfiguration config, IUserService userService, CRUDContext context)
        {
            _config = config;
            _userService = userService;
            _context = context;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequestModel model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        //https://localhost:44301/users/authenticate/

        [HttpDelete("DeletebyID/{id}")]
        public IActionResult DeleteById(int id)
        {

            _userService.DeleteById(id);



            return Ok();
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisteredModel model)
        {
            if (_userService.Register(model) == 1)
                return Ok();
            else
                return BadRequest();


        }
        [HttpPut("EditRegister/{id}")]
        public IActionResult EditRegister(int id, RegisteredModel model)
        {
            if (_userService.EditRegister(id, model) == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }


        }
        /*
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
           // 


        }*/

    }
}
