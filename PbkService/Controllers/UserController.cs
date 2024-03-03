using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return Ok(new RegisterViewModel());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                await _service.Create(model);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Ok(new LoginViewModel());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool resultAuth = await _service.Authenticate(model);
            if (resultAuth)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
