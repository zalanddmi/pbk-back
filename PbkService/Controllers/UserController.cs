using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService service) : Controller
    {
        private readonly UserService _service = service;

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
            string result = await _service.Authenticate(model);
            if (result == null)
            {
                return Unauthorized("Ошибка при вводе учетных данных");
            }
            return Ok(new {token = result});
        }
    }
}
