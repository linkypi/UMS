using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ProMainInfoMap : EntityTypeConfiguration<Pro_ProMainInfo>
    {
        public Pro_ProMainInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProMainID);

            // Properties
            this.Property(t => t.ProMainName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ProMainInfo");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ProMainName).HasColumnName("ProMainName");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ProNameID).HasColumnName("ProNameID");
            this.Property(t => t.Introduction).HasColumnName("Introduction");

            // Relationships
            this.HasOptional(t => t.Pro_ProNameInfo)
                .WithMany(t => t.Pro_ProMainInfo)
                .HasForeignKey(d => d.ProNameID);

        }
    }
}
