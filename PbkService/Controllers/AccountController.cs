using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AccountService accountService) : Controller
    {
        private readonly AccountService _accountService = accountService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                UserDTO userDTO = await _accountService.Login(request);
                return Ok(userDTO);
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
            catch (UserEmailNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserEmailNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (UserPhonenumberNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserPhonenumberNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (InvalidUserPassword ex)
            {
                Error error = new()
                {
                    Code = nameof(InvalidUserPassword),
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                UserDTO userDTO = await _accountService.Register(request);
                return Ok(userDTO);
            }
            catch (UserUsernameExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserUsernameExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (UserEmailExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserEmailExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (UserPhonenumberExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserPhonenumberExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch(Exception ex)
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
