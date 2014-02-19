using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_KeepLoreMap : EntityTypeConfiguration<VIP_KeepLore>
    {
        public VIP_KeepLoreMap()
        {
            // Primary Key
            this.HasKey(t => t.keepLoreId);

            // Properties
            this.Property(t => t.keepLoreTitle)
                .HasMaxLength(150);

            this.Property(t => t.keepLoreAbstract)
                .HasMaxLength(250);

            this.Property(t => t.keepLoreInfor)
                .HasMaxLength(350);

            this.Property(t => t.keepLorePicbig)
                .HasMaxLength(100);

            this.Property(t => t.keepLorePicsmall)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_KeepLore");
            this.Property(t => t.keepLoreId).HasColumnName("keepLoreId");
            this.Property(t => t.keepLoreTitle).HasColumnName("keepLoreTitle");
            this.Property(t => t.keepLoreAbstract).HasColumnName("keepLoreAbstract");
            this.Property(t => t.keepLoreInfor).HasColumnName("keepLoreInfor");
            this.Property(t => t.keepLorePicbig).HasColumnName("keepLorePicbig");
            this.Property(t => t.keepLorePicsmall).HasColumnName("keepLorePicsmall");
            this.Property(t => t.keepLoreTime).HasColumnName("keepLoreTime");
        }
    }
}
