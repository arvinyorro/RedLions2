namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class InquiryChatSessionMap : EntityTypeConfiguration<InquiryChatSession>
    {
        public InquiryChatSessionMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties.
            this.Property(t => t.InquirerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.StartedDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            this.Property(t => t.LastMessageDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Properties.
            this.HasRequired(t => t.Member)
                .WithMany()
                .Map(m => m.MapKey("member_id"));

            this.HasMany(t => t.ChatMessages)
                .WithRequired(t => t.InquiryChatSession);

            // Table and Columns Mappings.
            this.ToTable("inquiry_chat_sessions");
            this.Property(t => t.ID).HasColumnName("inquiry_chat_session_id");
            this.Property(t => t.InquirerName).HasColumnName("inquirer_name");
            this.Property(t => t.StartedDateTime).HasColumnName("datetime_started");
            this.Property(t => t.LastMessageDateTime).HasColumnName("datetime_lastmessage");
        }
    }
}
