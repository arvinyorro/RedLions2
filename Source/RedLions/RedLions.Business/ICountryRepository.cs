namespace RedLions.Business
{
    using System.Collections.Generic;

    public interface ICountryRepository
    {
        Country GetByID(int id);
        IEnumerable<Country> GetAll();
    }
}
