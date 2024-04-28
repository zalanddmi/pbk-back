using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries.Exceptions.Bank;
using PbkService.Auxiliaries;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels.Cards;
using PbkService.Auxiliaries.Exceptions.Card;
using PbkService.Auxiliaries.Exceptions.TypeCard;
using PbkService.Auxiliaries.Exceptions.PbkCategory;
using PbkService.Auxiliaries.Exceptions.Cashback;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    public class CardController(CardService cardService) : Controller
    {
        private readonly CardService _cardService = cardService;

        [HttpGet]
        [Authorize]
        public IActionResult GetPaged([FromQuery] GetPagedRequest request)
        {
            try
            {
                PbkPagedList<CardDTO> cards = _cardService.GetPagedList(request);
                return Ok(cards);
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
                CardDTO card = _cardService.GetById(id);
                return Ok(card);
            }
            catch (CardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(CardNotExists),
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
        public IActionResult Create(CardDTO card)
        {
            try
            {
                int id = _cardService.Create(card);
                return Ok(id);
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
            catch (TypeCardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(TypeCardNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (PbkCategoryNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(PbkCategoryNotExists),
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
        [Authorize]
        public IActionResult Update(CardDTO card)
        {
            try
            {
                _cardService.Update(card);
                return Ok();
            }
            catch (CardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(CardNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
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
            catch (TypeCardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(TypeCardNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (PbkCategoryNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(PbkCategoryNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (CashbackNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(CashbackNotExists),
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
                _cardService.Delete(id);
                return Ok();
            }
            catch (CardNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(CardNotExists),
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
