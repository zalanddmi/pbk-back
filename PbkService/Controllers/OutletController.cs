using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries.Exceptions.Shop;
using PbkService.Auxiliaries;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels;
using PbkService.Auxiliaries.Exceptions.Outlet;
using PbkService.Auxiliaries.Exceptions.Mcc;

namespace PbkService.Controllers
{
    [Route("api/operator/[controller]")]
    [ApiController]
    [Authorize]
    public class OutletController(OutletService outletService) : Controller
    {
        private readonly OutletService _outletService = outletService;

        [HttpGet]
        public IActionResult GetPagedList([FromQuery] GetPagedRequest request)
        {
            try
            {
                PbkPagedList<OutletDTO> outlets = _outletService.GetPagedList(request);
                return Ok(outlets);
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
                OutletDTO outlet = _outletService.GetById(id);
                return Ok(outlet);
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
        [Authorize(Roles = "Operator")]
        public IActionResult Create(OutletDTO outlet)
        {
            try
            {
                int id = _outletService.Create(outlet);
                return Ok(id);
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
            catch (MccNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(MccNotExists),
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
        [Authorize(Roles = "Operator")]
        public IActionResult Update(OutletDTO outlet)
        {
            try
            {
                _outletService.Update(outlet);
                return Ok();
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
            catch (ShopNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(ShopNotExists),
                    Message = ex.Message
                };
                return BadRequest(error);
            }
            catch (MccNotExists ex)
            {
                Error error = new()
                {
                    Code = nameof(MccNotExists),
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
        [Authorize(Roles = "Operator")]
        public IActionResult Delete(int id)
        {
            try
            {
                _outletService.Delete(id);
                return Ok();
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
