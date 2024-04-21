using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    public class ShopController(ShopService shopService) : Controller
    {
        private readonly ShopService _shopService = shopService;

        [HttpGet]
        [Authorize]
        public IActionResult GetShops()
        {
            try
            {
                IEnumerable<ShopDTO> shops = _shopService.GetShops();
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
        [Authorize]
        public IActionResult GetShopById(int id)
        {
            try
            {
                ShopDTO shop = _shopService.GetShopById(id);
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
