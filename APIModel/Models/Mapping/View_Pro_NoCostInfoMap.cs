using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_NoCostInfoMap : EntityTypeConfiguration<View_Pro_NoCostInfo>
    {
        public View_Pro_NoCostInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.InListID);

            // Properties
            this.Property(t => t.InListID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ClassID)
                .HasMaxLength(100);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_NoCostInfo");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
        }
    }
}
