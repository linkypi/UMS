using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPOffListAduitMap : EntityTypeConfiguration<View_VIPOffListAduit>
    {
        public View_VIPOffListAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Type, t.ArriveMoney, t.HeadID });

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ArriveMoney)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.SalesName)
                .HasMaxLength(50);

            this.Property(t => t.HeadID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_VIPOffListAduit");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ArriveMoney).HasColumnName("ArriveMoney");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.VIPTicketMaxCount).HasColumnName("VIPTicketMaxCount");
            this.Property(t => t.SalesName).HasColumnName("SalesName");
            this.Property(t => t.HeadID).HasColumnName("HeadID");
        }
    }
}
