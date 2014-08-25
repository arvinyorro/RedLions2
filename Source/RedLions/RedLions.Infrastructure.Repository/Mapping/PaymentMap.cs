namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.PaymentTypeID)
                .IsRequired();

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.MiddleName)
                .IsOptional()
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Age)
                .IsRequired();

            this.Property(t => t.Gender)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MobileNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.PublicID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ReferenceNumber)
                .IsOptional()
                .HasMaxLength(100);

            this.Property(t => t.AdminUnread)
                .IsRequired()
                .HasColumnType("bit");

            this.Property(t => t.ReferrerUnread)
                .IsRequired()
                .HasColumnType("bit");

            this.Property(t => t.CreatedDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            this.Property(t => t.BirthDate)
                .IsRequired()
                .HasColumnType("datetime");

            this.Ignore(t => t.Type);

            // Navigational Models
            this.HasRequired(t => t.Referrer)
                .WithMany()
                .Map(m => m.MapKey("referrer_user_id"));

            this.HasMany(t => t.GiftCertificates)
                .WithRequired(t => t.Payment);

            // Column and Table Mappings
            this.ToTable("payments");
            this.Property(t => t.ID).HasColumnName("payment_id");
            this.Property(t => t.PaymentTypeID).HasColumnName("payment_type_id");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t.FirstName).HasColumnName("first_name");
            this.Property(t => t.MiddleName).HasColumnName("middle_name");
            this.Property(t => t.LastName).HasColumnName("last_name");
            this.Property(t => t.Age).HasColumnName("age");
            this.Property(t => t.Gender).HasColumnName("gender");
            this.Property(t => t.PaymentMethod).HasColumnName("payment_method");
            this.Property(t => t.MobileNumber).HasColumnName("mobile_number");
            this.Property(t => t.Address).HasColumnName("address");
            this.Property(t => t.PublicID).HasColumnName("public_id");
            this.Property(t => t.ReferenceNumber).HasColumnName("reference_number");
            this.Property(t => t.CreatedDateTime).HasColumnName("datetime_created");
            this.Property(t => t.BirthDate).HasColumnName("date_birth");
            this.Property(t => t.AdminUnread).HasColumnName("admin_unread");
            this.Property(t => t.ReferrerUnread).HasColumnName("referrer_unread");
        }
    }
}
