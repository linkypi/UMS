using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProBandInfoMap : EntityTypeConfiguration<View_ProBandInfo>
    {
        public View_ProBandInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Order);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Value)
                .HasMaxLength(50);

            this.Property(t => t.Order)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_ProBandInfo");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Order).HasColumnName("Order");
        }
    }
}
