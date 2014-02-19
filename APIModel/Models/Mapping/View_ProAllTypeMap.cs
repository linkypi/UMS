using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProAllTypeMap : EntityTypeConfiguration<View_ProAllType>
    {
        public View_ProAllTypeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProID, t.SellTypeID });

            // Properties
            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.TypeID)
                .HasMaxLength(100);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassID)
                .HasMaxLength(100);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            this.Property(t => t.SellTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_ProAllType");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
        }
    }
}
