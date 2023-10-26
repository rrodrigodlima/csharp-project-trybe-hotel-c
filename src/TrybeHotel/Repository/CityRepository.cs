using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            var cities = _context.Cities
                .Select(city => new CityDto
                {
                    CityId = city.CityId,
                    Name = city.Name,
                    State = city.State
                })
                .ToList();

            return cities;
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            // Mapeia a nova cidade para o formato CityDto e retorna >>
            var addedCityDto = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };

            return addedCityDto;
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var selectedCity = _context.Cities!.FirstOrDefault(c => c.CityId == city.CityId)
                             ?? throw new Exception("Cidade não encontrada");

            selectedCity.Name = city.Name;
            selectedCity.State = city.State;

            _context.SaveChanges();

            return new CityDto
            {
                CityId = selectedCity.CityId,
                Name = selectedCity.Name,
                State = selectedCity.State
            };
        }

    }
}