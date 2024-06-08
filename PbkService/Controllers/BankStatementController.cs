using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Services;

namespace PbkService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("bank-statement")]
    public class BankStatementController(BankStatementParserService service) : Controller
    {
        [HttpPost("parse")]
        public IActionResult ParseStatement(IFormFile file)
        {
            string? username = HttpContext?.User?.Identity?.Name;
            try
            {
                var operations = service.ParseStatement(username, file);
                return Ok(operations);
            }
            catch (UserUsernameNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserUsernameNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                Error error = new()
                {
                    Message = ex.Message
                };
                return BadRequest(error);
            }
        }
    }
}
