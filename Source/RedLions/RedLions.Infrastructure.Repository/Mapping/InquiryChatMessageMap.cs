namespace RedLions.Infrastructure.Repository.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using RedLions.Business;

    public class InquiryChatMessageMap : EntityTypeConfiguration<InquiryChatMessage>
    {
        public InquiryChatMessageMap()
        {
            // Primary Key.
            this.HasKey(t => t.ID);
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties.
            this.Property(t => t.SenderUsername)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.SentDateTime)
                .IsRequired()
                .HasColumnType("datetime");

            // Navigational Models.
            this.HasRequired(t => t.InquiryChatSession)
                .WithMany(t => t.ChatMessages)
                .Map(m => m.MapKey("inquiry_chat_session_id"));

            // Table and Columns Mappings.
            this.ToTable("inquiry_chat_messages");
            this.Property(t => t.ID).HasColumnName("inquiry_chat_message_id");
            this.Property(t => t.SenderUsername).HasColumnName("sender_username");
            this.Property(t => t.Message).HasColumnName("message");
            this.Property(t => t.SentDateTime).HasColumnName("datetime_sent");
        }
    }
}
