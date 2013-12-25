namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.RegisteredDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Models
            this.Property(t => t.Role)
                .IsRequired();

            // Column and Table Mappings
            this.ToTable("users");
            this.Property(t => t.ID).HasColumnName("user_id");
            this.Property(t => t.Role).HasColumnName("role_id");
            this.Property(t => t.Username).HasColumnName("username");
            this.Property(t => t.Password).HasColumnName("password");
            this.Property(t => t.FirstName).HasColumnName("first_name");
            this.Property(t => t.LastName).HasColumnName("last_name");
            this.Property(t => t.RegisteredDateTime).HasColumnName("datetime_registered");
        }
    }
}
