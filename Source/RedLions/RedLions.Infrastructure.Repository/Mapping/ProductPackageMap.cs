namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class ProductPackageMap : EntityTypeConfiguration<ProductPackage>
    {
        public ProductPackageMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CreatedDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Model
            this.HasMany(t => t.Payments)
                .WithRequired(t => t.Package);

            // Table and Column Mappings
            this.ToTable("product_packages");
            this.Property(t => t.ID).HasColumnName("product_package_id");
            this.Property(t => t.Title).HasColumnName("title");
            this.Property(t => t.CreatedDateTime).HasColumnName("datetime_created");
        }
    }
}
