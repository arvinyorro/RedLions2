namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class PaymentGiftMap : EntityTypeConfiguration<PaymentGift>
    {
        public PaymentGiftMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Quanitity)
                .IsRequired();

            this.Property(t => t.Price)
                .IsRequired()
                .HasColumnType("decimal");

            // Navigational Model
            this.HasRequired(t => t.Payment)
                .WithMany(t => t.GiftCertificates)
                .Map(m => m.MapKey("payment_id"));

            // Table and Column Mappings
            this.ToTable("payment_gifts");
            this.Property(t => t.ID).HasColumnName("payment_gift_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.Quanitity).HasColumnName("quantity");
            this.Property(t => t.Price).HasColumnName("price");
        }
    }
}
