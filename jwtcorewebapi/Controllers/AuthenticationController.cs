using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using jwtcorewebapi.Model;
using jwtcorewebapi.Services;

namespace jwtcorewebapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public IActionResult post([FromBody]User user)
        {
            var userdata = _userService.Authenticate(user.UserName, user.Password);
            if (userdata == null)
                return BadRequest(new { message = "username or password is in correct" });
            return Ok(userdata);
        }
    }
}