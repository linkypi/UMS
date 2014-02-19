using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_tianYidiscountMap : EntityTypeConfiguration<VIP_tianYidiscount>
    {
        public VIP_tianYidiscountMap()
        {
            // Primary Key
            this.HasKey(t => t.tianYidiscountId);

            // Properties
            this.Property(t => t.tianYidiscountPic)
                .HasMaxLength(50);

            this.Property(t => t.tianYidiscountPicbigid)
                .HasMaxLength(50);

            this.Property(t => t.tianYidiscountSynopsis)
                .HasMaxLength(250);

            this.Property(t => t.tianYidiscountInfo)
                .HasMaxLength(250);

            this.Property(t => t.tianYidiscountName)
                .HasMaxLength(150);

            this.Property(t => t.tianYidiscountPrice)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_tianYidiscount");
            this.Property(t => t.tianYidiscountId).HasColumnName("tianYidiscountId");
            this.Property(t => t.tianYidiscountPic).HasColumnName("tianYidiscountPic");
            this.Property(t => t.tianYidiscountPicbigid).HasColumnName("tianYidiscountPicbigid");
            this.Property(t => t.tianYidiscountSynopsis).HasColumnName("tianYidiscountSynopsis");
            this.Property(t => t.tianYidiscountInfo).HasColumnName("tianYidiscountInfo");
            this.Property(t => t.tianYidiscountName).HasColumnName("tianYidiscountName");
            this.Property(t => t.tianYidiscountPrice).HasColumnName("tianYidiscountPrice");
        }
    }
}
