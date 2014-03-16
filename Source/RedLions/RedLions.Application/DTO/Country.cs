namespace RedLions.Application.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    public class Country
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }

    internal static class CountryAssembler
    {
        internal static IEnumerable<DTO.Country> ToDTOList(this IEnumerable<Business.Country> countries)
        {
            return countries.Select(x => CountryAssembler.ToDTO(x));
        }

        internal static DTO.Country ToDTO(this Business.Country country)
        {
            var CountryDTO = new DTO.Country()
            {
                ID = country.ID,
                Title = country.Title,
            };

            return CountryDTO;
        }
    }
}
