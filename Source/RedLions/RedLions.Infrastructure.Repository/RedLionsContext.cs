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
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<MemberPointsLog> MemberPointsLogs { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentGift> PaymentGifts { get; set; }
        public DbSet<ProductPackage> ProductPackages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new InquiryMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new InquiryChatMessageMap());
            modelBuilder.Configurations.Add(new InquiryChatSessionMap());
            modelBuilder.Configurations.Add(new SubscriptionMap());
            modelBuilder.Configurations.Add(new AnnouncementMap());
            modelBuilder.Configurations.Add(new MemberPointsLogMap());
            modelBuilder.Configurations.Add(new PaymentMap());
            modelBuilder.Configurations.Add(new PaymentGiftMap());
            modelBuilder.Configurations.Add(new ProductPackageMap());
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}