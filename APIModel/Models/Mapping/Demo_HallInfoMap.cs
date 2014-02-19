using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_HallInfoMap : EntityTypeConfiguration<Demo_HallInfo>
    {
        public Demo_HallInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallClass)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_HallInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallClass).HasColumnName("HallClass");
            this.Property(t => t.HallName).HasColumnName("HallName");
        }
    }
}
