using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ProNameInfoMap : EntityTypeConfiguration<Pro_ProNameInfo>
    {
        public Pro_ProNameInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.NameID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.MainName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ProNameInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.NameID).HasColumnName("NameID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.MainName).HasColumnName("MainName");
        }
    }
}
