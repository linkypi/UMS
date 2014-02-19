using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPService_BAKMap : EntityTypeConfiguration<VIP_VIPService_BAK>
    {
        public VIP_VIPService_BAKMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPService_BAK");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.OldVIPID).HasColumnName("OldVIPID");
            this.Property(t => t.NewVIPID).HasColumnName("NewVIPID");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SCount).HasColumnName("SCount");
        }
    }
}
