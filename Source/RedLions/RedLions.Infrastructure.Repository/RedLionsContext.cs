// -----------------------------------------------------------------------
// <copyright file="EtsContext.cs" company="SPi Global">
// Developer: Arvin Yorro
// </copyright>
// -----------------------------------------------------------------------

namespace RedLions.Infrastructure.Repository
{
    using System.Data.Entity;
    using RedLions.Infrastructure.Repository.Mapping;
    using RedLions.CrossCutting;
    using RedLions.Business;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RedLionsContext : DbContext, IDbContext
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
        public DbSet<InquiryChatSession> InquiryChatSessions { get; set; }
        public DbSet<InquiryChatMessage> InquiryChatMessages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new InquiryMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new InquiryChatMessageMap());
            modelBuilder.Configurations.Add(new InquiryChatSessionMap());
            modelBuilder.Configurations.Add(new SubscriptionMap());
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}