using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPServiceMap : EntityTypeConfiguration<VIP_VIPService>
    {
        public VIP_VIPServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPService");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.VIPID).HasColumnName("VIPID");
            this.Property(t => t.SCount).HasColumnName("SCount");
            this.Property(t => t.UsedCount).HasColumnName("UsedCount");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.VIP_VIPService)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_VIPService)
                .HasForeignKey(d => d.VIPID);
            this.HasOptional(t => t.VIP_VIPInfo_Temp)
                .WithMany(t => t.VIP_VIPService)
                .HasForeignKey(d => d.VIPID);

        }
    }
}
