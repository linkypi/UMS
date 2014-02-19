using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_AreaInfoMap : EntityTypeConfiguration<Pro_AreaInfo>
    {
        public Pro_AreaInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.AreaID);

            // Properties
            this.Property(t => t.AreaName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.MapColor)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("Pro_AreaInfo");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Points).HasColumnName("Points");
            this.Property(t => t.MapColor).HasColumnName("MapColor");
            this.Property(t => t.BigAreaID).HasColumnName("BigAreaID");

            // Relationships
            this.HasOptional(t => t.Pro_BigAreaInfo)
                .WithMany(t => t.Pro_AreaInfo)
                .HasForeignKey(d => d.BigAreaID);

        }
    }
}
