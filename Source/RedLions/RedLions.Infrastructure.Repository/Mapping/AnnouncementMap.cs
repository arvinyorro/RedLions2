namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class AnnouncementMap : EntityTypeConfiguration<Announcement>
    {
        public AnnouncementMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(600);

            this.Property(t => t.PostedDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Models
            this.HasRequired(t => t.UserPoster)
                .WithMany()
                .Map(m => m.MapKey("user_id"));

            // Table and Column Mappings
            this.ToTable("announcements");
            this.Property(t => t.ID).HasColumnName("announcement_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.Message).HasColumnName("message");
            this.Property(t => t.PostedDateTime).HasColumnName("datetime_posted");
        }
    }
}
