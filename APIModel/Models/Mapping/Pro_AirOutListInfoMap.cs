using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_AirOutListInfoMap : EntityTypeConfiguration<Pro_AirOutListInfo>
    {
        public Pro_AirOutListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ListID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.OldProID)
                .HasMaxLength(50);

            this.Property(t => t.NewProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.NewInListID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_AirOutListInfo");
            this.Property(t => t.ListID).HasColumnName("ListID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.OldProID).HasColumnName("OldProID");
            this.Property(t => t.NewProID).HasColumnName("NewProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.AirOutID).HasColumnName("AirOutID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.NewInListID).HasColumnName("NewInListID");

            // Relationships
            this.HasOptional(t => t.Pro_AirOutInfo)
                .WithMany(t => t.Pro_AirOutListInfo)
                .HasForeignKey(d => d.AirOutID);

        }
    }
}
