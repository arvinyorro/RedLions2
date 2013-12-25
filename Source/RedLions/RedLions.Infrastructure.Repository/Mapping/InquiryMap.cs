namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class InquiryMap : EntityTypeConfiguration<Inquiry>
    {
        public InquiryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CellphoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.InquiredDataTime)
                .IsRequired()
                .HasColumnType("datetime");

            this.Property(t => t.Registered)
                .IsRequired()
                .HasColumnType("bit");

            // Navigational Models
            this.HasRequired(t => t.Referrer)
                .WithMany()
                .Map(m => m.MapKey("member_id"));

            // Column and Table Mappings
            this.ToTable("inquiries");
            this.Property(t => t.ID).HasColumnName("inquiry_id");
            this.Property(t => t.FirstName).HasColumnName("first_name");
            this.Property(t => t.LastName).HasColumnName("last_name");
            this.Property(t => t.CellphoneNumber).HasColumnName("cellphone_number");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t.InquiredDataTime).HasColumnName("datetime_inquired");
        }
    }
}
