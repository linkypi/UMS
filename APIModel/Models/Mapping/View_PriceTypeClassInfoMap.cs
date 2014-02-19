using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_PriceTypeClassInfoMap : EntityTypeConfiguration<View_PriceTypeClassInfo>
    {
        public View_PriceTypeClassInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProID, t.TypeID, t.ClassID });

            // Properties
            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_PriceTypeClassInfo");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
        }
    }
}
