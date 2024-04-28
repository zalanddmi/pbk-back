using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Bank;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    public class BankController(BankService bankService) : Controller
    {
        private readonly BankService _bankService = bankService;

        [HttpGet]
        [Authorize]
        public IActionResult GetPagedList([FromQuery] GetPagedRequest request)
        {
            try
            {
                PbkPagedList<BankDTO> banks = _bankService.GetPagedList(request);
                return Ok(banks);
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

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            try
            {
                BankDTO bank = _bankService.GetById(id);
                return Ok(bank);
            }
            catch (BankNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(BankNotExists),
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
        [Authorize]
        public IActionResult Create(BankDTO bank)
        {
            try
            {
                int id = _bankService.Create(bank);
                return Ok(id);
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
        [Authorize]
        public IActionResult Update(BankDTO bank) 
        {
            try
            {
                _bankService.Update(bank);
                return Ok();
            }
            catch (BankNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(BankNotExists),
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
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _bankService.Delete(id);
                return Ok();
            }
            catch (BankNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(BankNotExists),
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
