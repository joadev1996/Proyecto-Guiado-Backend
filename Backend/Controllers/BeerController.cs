using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        
        private IValidator<BeerInsertDTO> _beerInsertValidator;
        private IValidator<BeerUpdateDTO> _beerUpdateValidator;
        private ICommonService<BeerDTO, BeerInsertDTO,BeerUpdateDTO> _beerServices;
        public BeerController(
            IValidator<BeerInsertDTO> beerInsertValidator,
            IValidator<BeerUpdateDTO> beerUpdateValidator,
           [FromKeyedServices("beerService")] ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO> beerService)
        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerServices = beerService;

        }

        [HttpGet]
        public async Task<IEnumerable<BeerDTO>> Get() =>
         await _beerServices.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDTO>> GetByID(int id)
        {
            var beerDTO = await _beerServices.GetById(id);

            return beerDTO == null ? NotFound() : Ok(beerDTO);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDTO>> Add(BeerInsertDTO beerInsertDTO)
        {
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDTO);

            if (!validationResult.IsValid) { return BadRequest(validationResult.Errors);  }

            if (!_beerServices.Validate(beerInsertDTO))
            {
                return BadRequest(_beerServices.Errors);
            }


            var beerDTO = await _beerServices.Add(beerInsertDTO);



            return CreatedAtAction(nameof(GetByID), new { id = beerDTO.Id }, beerDTO);
        }

        [HttpPut]
        public async Task<ActionResult<BeerDTO>> Update(int id, BeerUpdateDTO beerUpdateDTO)
        {
            var validationResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_beerServices.Validate(beerUpdateDTO))
            {
                return BadRequest(_beerServices.Errors);
            }

            var beerDTO = await _beerServices.Update(id, beerUpdateDTO);

            return beerDTO == null ? NotFound() : Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDTO>> Delete(int id)
        {
            var beerDTO = await _beerServices.Delete(id);

            return beerDTO == null ? NotFound() : Ok(beerDTO);
        }
    }
}