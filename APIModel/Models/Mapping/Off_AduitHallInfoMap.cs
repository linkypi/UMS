using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Off_AduitHallInfoMap : EntityTypeConfiguration<Off_AduitHallInfo>
    {
        public Off_AduitHallInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Off_AduitHallInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitTypeID).HasColumnName("AduitTypeID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Off_AduitHallInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Off_AduitTypeInfo)
                .WithMany(t => t.Off_AduitHallInfo)
                .HasForeignKey(d => d.AduitTypeID);

        }
    }
}
