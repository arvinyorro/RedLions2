namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class MemberPointsLogMap : EntityTypeConfiguration<MemberPointsLog>
    {
        public MemberPointsLogMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Points);

            this.Property(t => t.LoggedDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Properties
            this.HasRequired(t => t.AdminUser)
                .WithMany()
                .Map(m => m.MapKey("admin_user_id"));

            this.HasRequired(t => t.Member)
                .WithMany(t => t.MemberPointsLogs)
                .Map(m => m.MapKey("member_id"));

            // Table and Column mappings.
            this.ToTable("member_points_log");
            this.Property(t => t.ID).HasColumnName("member_points_log_id");
            this.Property(t => t.LoggedDateTime).HasColumnName("datetime_logged");
        }
    }
}
