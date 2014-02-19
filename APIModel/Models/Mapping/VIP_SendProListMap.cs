using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_SendProListMap : EntityTypeConfiguration<VIP_SendProList>
    {
        public VIP_SendProListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_SendProList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.SendID).HasColumnName("SendID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
        }
    }
}
