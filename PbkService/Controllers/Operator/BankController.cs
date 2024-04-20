using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Bank;
using PbkService.Auxiliaries.Exceptions.User;
using PbkService.Models;
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
        public IActionResult GetBanks()
        {
            try
            {
                IEnumerable<BankDTO> banks = _bankService.GetBanks();
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
        public IActionResult GetBankById(int id)
        {
            try
            {
                BankDTO bank = _bankService.GetBankById(id);
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
