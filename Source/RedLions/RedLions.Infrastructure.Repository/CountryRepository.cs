namespace RedLions.Infrastructure.Repository
{
    using System.Collections.Generic;
    using RedLions.Business;
    using RedLions.CrossCutting;

    public class CountryRepository : GenericRepository, ICountryRepository
    {
        public CountryRepository(IDbContext dbContext)
            : base(dbContext)
        {

        }

        public Country GetByID(int id)
        {
            return base.GetById<Country>(id);
        }

        public IEnumerable<Country> GetAll()
        {
            return base.GetAll<Country>();
        }
    }
}
