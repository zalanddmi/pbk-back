using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.Mcc;
using PbkService.Auxiliaries.Exceptions.PbkCategory;
using PbkService.Requests;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    public class PbkCategoryController(PbkCategoryService categoryService) : Controller
    {
        private readonly PbkCategoryService _categoryService = categoryService;

        [HttpGet]
        [Authorize]
        public IActionResult GetPagedCategories([FromQuery] GetPagedRequest request)
        {
            try
            {
                PbkPagedList<PbkCategoryDTO> categories = _categoryService.GetPagedList(request);
                return Ok(categories);
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
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                PbkCategoryDTO category = _categoryService.GetById(id);
                return Ok(category);
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

        [HttpPost]
        [Authorize]
        public IActionResult Create(PbkCategoryDTO category)
        {
            try
            {
                int id = _categoryService.Create(category);
                return Ok(id);
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
        [Authorize]
        public IActionResult Update(PbkCategoryDTO category)
        {
            try
            {
                _categoryService.Update(category);
                return Ok();
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

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _categoryService.Delete(id);
                return Ok();
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
    }
}
