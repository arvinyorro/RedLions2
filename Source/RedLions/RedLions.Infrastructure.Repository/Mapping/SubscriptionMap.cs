namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class SubscriptionMap : EntityTypeConfiguration<Subscription>
    {
        public SubscriptionMap()
        {
            // Primary Key
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Months)
                .IsRequired();

            // Column and Table Mappings
            this.ToTable("subscriptions");
            this.Property(t => t.ID).HasColumnName("subscription_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.Months).HasColumnName("months");
        }
    }
}
