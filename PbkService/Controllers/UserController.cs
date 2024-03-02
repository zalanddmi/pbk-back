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
            await _service.Create(model);
            return Ok(model);
        }
    }
}
