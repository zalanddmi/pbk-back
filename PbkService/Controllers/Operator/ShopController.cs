using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    [Authorize(Roles = "Operator")]
    public class ShopController(ShopService shopService) : Controller
    {
        private readonly ShopService _shopService = shopService;

        [HttpGet]
        public IActionResult GetPagedList([FromQuery] GetPagedRequest request)
        {
            try
            {
                PbkPagedList<ShopDTO> shops = _shopService.GetPagedList(request);
                return Ok(shops);
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
        public IActionResult GetById(int id)
        {
            try
            {
                ShopDTO shop = _shopService.GetById(id);
                return Ok(shop);
            }
            catch (ShopNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(ShopNotExists),
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
        public IActionResult Create(ShopDTO shop)
        {
            try
            {
                int id = _shopService.Create(shop);
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
        public IActionResult Update(ShopDTO shop)
        {
            try
            {
                _shopService.Update(shop);
                return Ok();
            }
            catch (ShopNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(ShopNotExists),
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
                _shopService.Delete(id);
                return Ok();
            }
            catch (ShopNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(ShopNotExists),
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
