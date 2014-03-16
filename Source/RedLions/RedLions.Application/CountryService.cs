namespace RedLions.Application
{
    using System;
    using System.Collections.Generic;
    using RedLions.Application.DTO;
    using RedLions.Business;

    public class CountryService
    {
        private ICountryRepository countryRepository;

        public CountryService(
            ICountryRepository countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public IEnumerable<DTO.Country> GetAll()
        {
            return this.countryRepository
                .GetAll()
                .ToDTOList();
        }

        public DTO.Country GetByID(int id)
        {
            return this.countryRepository.GetByID(id).ToDTO();
        }

        public DTO.Country GetDefaultCountry()
        {
            int philippinesID = 146;
            Business.Country country = this.countryRepository.GetByID(philippinesID);
            if (country == null)
            {
                throw new Exception("Philippines country not found");
            }

            return country.ToDTO();
        }
    }
}
