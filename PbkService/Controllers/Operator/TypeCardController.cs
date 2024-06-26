﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PbkService.Auxiliaries;
using PbkService.Auxiliaries.Exceptions.TypeCard;
using PbkService.Services;
using PbkService.ViewModels;

namespace PbkService.Controllers.Operator
{
    [Route("api/operator/[controller]")]
    [ApiController]
    [Authorize(Roles = "Operator")]
    public class TypeCardController(TypeCardService typeCardService) : Controller
    {
        private readonly TypeCardService _typeCardService = typeCardService;

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<TypeCardDTO> typeCards = _typeCardService.Get();
                return Ok(typeCards);
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
                TypeCardDTO typeCard = _typeCardService.GetById(id);
                return Ok(typeCard);
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
        public IActionResult Create(TypeCardDTO typeCard)
        {
            try
            {
                int id = _typeCardService.Create(typeCard);
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
        public IActionResult Update(TypeCardDTO typeCard)
        {
            try
            {
                _typeCardService.Update(typeCard);
                return Ok();
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
                _typeCardService.Delete(id);
                return Ok();
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
