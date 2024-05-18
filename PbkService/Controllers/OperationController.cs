using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries.Exceptions.Outlet;
using PbkService.Auxiliaries;
using PbkService.Services;
using PbkService.ViewModels;
using PbkService.Auxiliaries.Exceptions.Operation;
using PbkService.Auxiliaries.Exceptions.User;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperationController(OperationService operationService) : Controller
    {
        private readonly OperationService _operationService = operationService;

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                OperationDTO operation = _operationService.GetById(id);
                return Ok(operation);
            }
            catch (OperationNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationNotExists),
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
                List<OperationDTO> operations = _operationService.GetByUser(username);
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

        [HttpPost]
        public IActionResult Create(OperationDTO operation)
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                int id = _operationService.Create(operation, username);
                return Ok(id);
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
            catch (OutletNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OutletNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (OperationOutletExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationOutletExists),
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

        [HttpPost("list")]
        public IActionResult Create([FromBody] List<OperationDTO> operations)
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                List<int> ids = _operationService.Create(operations, username);
                return Ok(ids);
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
            catch (OutletNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OutletNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (OperationOutletExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationOutletExists),
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

        [HttpPut]
        public IActionResult Update(OperationDTO operation)
        {
            string? username = HttpContext.User.Identity.Name;
            try
            {
                _operationService.Update(operation, username);
                return Ok();
            }
            catch (OperationNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (OutletNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OutletNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
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
            catch (OperationOutletExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationOutletExists),
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _operationService.Delete(id);
                return Ok();
            }
            catch (OperationNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationNotExists),
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

        [HttpDelete("list")]
        public IActionResult Delete([FromBody] int[] ids)
        {
            try
            {
                _operationService.Delete(ids);
                return Ok();
            }
            catch (OperationNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(OperationNotExists),
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
