using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Auxiliaries.Exceptions.UserCard;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCardController(UserCardService userCardService) : Controller
    {
        private readonly UserCardService _userCardService = userCardService;

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                UserCardDTO card = _userCardService.GetById(id, username);
                return Ok(card);
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
            catch (UserCardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(UserCardNotExists),
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

        [HttpGet]
        public IActionResult GetByUser()
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                List<UserCardDTO> operations = _userCardService.GetByUser(username);
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

        [HttpGet("algorithm")]
        public IActionResult ExecuteAlgorithm()
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                List<UserCardDTO> operations = _userCardService.ExecuteAlgorithm(username);
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
