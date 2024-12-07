using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Services
{
    public class BeerService : ICommonService<BeerDTO, BeerInsertDTO, BeerUpdateDTO>
    {

        public List<string> Errors { get; }
        private IRepository<Beer> _beerRepository;
        private IMapper _mapper;

        public BeerService(IRepository<Beer> beerRepository,
            IMapper mapper)
        {
            _mapper = mapper;
            _beerRepository = beerRepository;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<BeerDTO>> Get() 
        {
            var beers = await _beerRepository.Get();

            return beers.Select(b => _mapper.Map<BeerDTO>(b));
        }

        public async Task<BeerDTO> GetById(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                var beerDTO =  _mapper.Map<BeerDTO>(beer);
                return beerDTO;
            }
            return null;
        }
        public async Task<BeerDTO> Add(BeerInsertDTO beerInsertDTO)
        {
            var beer = _mapper.Map<Beer>(beerInsertDTO);

            await _beerRepository.Add(beer);
            await _beerRepository.Save();

            var beerDto = _mapper.Map<BeerDTO>(beer);

            return beerDto;
        }
        public async Task<BeerDTO> Update(int id, BeerUpdateDTO beerUpdateDTO)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {
                beer.Name = beerUpdateDTO.Name;
                beer.Alcohol = beerUpdateDTO.Alcohol;
                beer.BrandID = beerUpdateDTO.BrandID;
                

                _beerRepository.Update(beer);  
                await _beerRepository.Save();

                var beerDto = _mapper.Map<BeerDTO>(beer);


                return beerDto;

            }

            return null;

        }
        public async Task<BeerDTO> Delete(int id)
        {
            var beer = await _beerRepository.GetById(id);

            if (beer != null)
            {

                var beerDto = _mapper.Map<BeerDTO>(beer);

                _beerRepository.Delete(beer);

                await _beerRepository.Save();



                return beerDto;

            }

            return null;
        }

        public bool Validate(BeerInsertDTO beerInsertDTO)
        {
            if (_beerRepository.Search(b => b.Name == beerInsertDTO.Name).Count() >0 )
            {
                Errors.Add("No puede existir una cerveza con un nombre ya existente");
                return false;
            }
            return true;

        }

        public bool Validate(BeerUpdateDTO beerUpdateDTO)
        {
            if (_beerRepository.Search(b => b.Name == beerUpdateDTO.Name && beerUpdateDTO.Id != b.BeerId).Count() > 0)
            {
                Errors.Add("No puede existir una cerveza con un nombre ya existente");
                return false;
            }
            return true;

        }


    }
}
