// -----------------------------------------------------------------------
// <copyright file="EtsContext.cs" company="SPi Global">
// Developer: Arvin Yorro
// </copyright>
// -----------------------------------------------------------------------

namespace RedLions.Infrastructure.Repository
{
    using System.Data.Entity;
    using RedLions.Infrastructure.Repository.Mapping;
    using RedLions.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RedLionsContext : DbContext
    {
        static RedLionsContext()
        {
            Database.SetInitializer<RedLionsContext>(null);
        }

        public RedLionsContext()
            : base("name=RedLions")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new InquiryMap());
            modelBuilder.Configurations.Add(new CountryMap());
        }
    }
}