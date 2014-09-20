namespace RedLions.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RedLions.CrossCutting;
    using RedLions.Business;

    public class ProductPackageRepository : GenericRepository, IProductPackageRepository
    {
        public ProductPackageRepository(IDbContext dbContext)
            : base(dbContext) { }

        
        public ProductPackage GetByID(int id)
        {
            return base.GetSingle<ProductPackage>(x => x.ID == id);
        }
    }
}
