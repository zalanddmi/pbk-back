using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Services;

namespace PbkService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MccController(MccService service) : Controller
    {
        private readonly MccService _service = service;

        [HttpGet]
        [Authorize]
        public IActionResult GetMcc()
        {
            return Ok(_service.GetAll());
        }

        /// <summary>
        /// Временное решение для импорта серверного файла с MCC-кодами
        /// </summary>
        [HttpPost("Import")]
        [Authorize]
        public IActionResult Import()
        {
            _service.LoadMccDataFromFileIfExists(@"Source\\Mcc");
            return Ok();
        }
    }
}
