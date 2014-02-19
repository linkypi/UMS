using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_OffListPicsMap : EntityTypeConfiguration<VIP_OffListPics>
    {
        public VIP_OffListPicsMap()
        {
            // Primary Key
            this.HasKey(t => t.offListpicsId);

            // Properties
            this.Property(t => t.offListPic)
                .HasMaxLength(50);

            this.Property(t => t.offListIndex)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_OffListPics");
            this.Property(t => t.offListpicsId).HasColumnName("offListpicsId");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.offListPic).HasColumnName("offListPic");
            this.Property(t => t.offListIndex).HasColumnName("offListIndex");
        }
    }
}
