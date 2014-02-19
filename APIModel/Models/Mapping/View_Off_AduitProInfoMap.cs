using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Off_AduitProInfoMap : EntityTypeConfiguration<View_Off_AduitProInfo>
    {
        public View_Off_AduitProInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProMainID, t.SellType, t.AduitTypeID });

            // Properties
            this.Property(t => t.ProMainID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.AduitTypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProMainName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Off_AduitProInfo");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.AduitTypeID).HasColumnName("AduitTypeID");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.ProMainName).HasColumnName("ProMainName");
        }
    }
}
