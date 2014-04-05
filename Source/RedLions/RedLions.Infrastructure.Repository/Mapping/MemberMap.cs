namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class MemberMap : EntityTypeConfiguration<Member>
    {
        public MemberMap()
        {
            this.Property(t => t.MemberID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
  
            // Properties
            this.Property(t => t.ReferralCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CellphoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnoID)
                .IsRequired()
                .HasMaxLength(50);

            // Navigational Models
            this.HasOptional(t => t.Referrer)
                .WithMany(t => t.Referrals)
                .Map(m => m.MapKey("referral_user_id"));
            
            this.HasOptional(t => t.Inquiry)
                .WithOptionalDependent()
                .Map(m => m.MapKey("inquiry_id"));

            this.HasRequired(t => t.Country)
                .WithMany()
                .Map(m => m.MapKey("country_id"));
            
            // Column and Table Mappings
            this.ToTable("member_details");
            this.Property(t => t.MemberID).HasColumnName("member_id");
            this.Property(t => t.ReferralCode).HasColumnName("referral_code");
            this.Property(t => t.CellphoneNumber).HasColumnName("cellphone_number");
            this.Property(t => t.UnoID).HasColumnName("uno_id");
        }
    }
}
