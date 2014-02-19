using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_LevelInfoMap : EntityTypeConfiguration<Pro_LevelInfo>
    {
        public Pro_LevelInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.LevelID);

            // Properties
            this.Property(t => t.LevelName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_LevelInfo");
            this.Property(t => t.LevelID).HasColumnName("LevelID");
            this.Property(t => t.LevelName).HasColumnName("LevelName");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
