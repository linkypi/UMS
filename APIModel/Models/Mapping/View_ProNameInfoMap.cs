using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProNameInfoMap : EntityTypeConfiguration<View_ProNameInfo>
    {
        public View_ProNameInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.MainName)
                .HasMaxLength(50);

            this.Property(t => t.NameID)
                .HasMaxLength(50);

            this.Property(t => t.OldNote)
                .HasMaxLength(500);

            this.Property(t => t.OldMainName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ProNameInfo");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.MainName).HasColumnName("MainName");
            this.Property(t => t.NameID).HasColumnName("NameID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OldNote).HasColumnName("OldNote");
            this.Property(t => t.OldMainName).HasColumnName("OldMainName");
        }
    }
}
