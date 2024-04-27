using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Requests;
using PbkService.Services;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    public class MccController(MccService service) : Controller
    {
        private readonly MccService _service = service;

        [HttpGet]
        [Authorize]
        public IActionResult GetPagedMccs([FromQuery] GetPagedRequest request)
        {
            try
            {
                return Ok(_service.GetPagedList(request));
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

        /// <summary>
        /// Временное решение для импорта серверного файла с MCC-кодами
        /// </summary>
        [HttpPost("import")]
        [Authorize(Roles = "Admin")]
        public IActionResult Import(IFormFile formFile)
        {
            try
            {
                _service.LoadMccDataFromFile(formFile);
                return Ok();
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
