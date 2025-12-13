using EAPI.Entities;

namespace EAPI.DTO
{
    public class IndexData
    {
        public IEnumerable<Country> CountryIndex { get; set; }
        public IEnumerable<City> CityIndex { get; set; }

    }
}
