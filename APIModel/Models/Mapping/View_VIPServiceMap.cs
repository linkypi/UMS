using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPServiceMap : EntityTypeConfiguration<View_VIPService>
    {
        public View_VIPServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_VIPService");
            this.Property(t => t.SCount).HasColumnName("SCount");
            this.Property(t => t.UsedCount).HasColumnName("UsedCount");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.VIPID).HasColumnName("VIPID");
        }
    }
}
