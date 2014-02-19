using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_SearchShopMap : EntityTypeConfiguration<VIP_SearchShop>
    {
        public VIP_SearchShopMap()
        {
            // Primary Key
            this.HasKey(t => t.shopId);

            // Properties
            this.Property(t => t.shopName)
                .HasMaxLength(50);

            this.Property(t => t.shopAdd)
                .HasMaxLength(100);

            this.Property(t => t.shopPhone)
                .HasMaxLength(50);

            this.Property(t => t.shopPicbig)
                .HasMaxLength(50);

            this.Property(t => t.shopPic)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_SearchShop");
            this.Property(t => t.shopId).HasColumnName("shopId");
            this.Property(t => t.shopName).HasColumnName("shopName");
            this.Property(t => t.shopAdd).HasColumnName("shopAdd");
            this.Property(t => t.shopPhone).HasColumnName("shopPhone");
            this.Property(t => t.shopPicbig).HasColumnName("shopPicbig");
            this.Property(t => t.shopPic).HasColumnName("shopPic");
            this.Property(t => t.Shoplongitude).HasColumnName("Shoplongitude");
            this.Property(t => t.shopLatitude).HasColumnName("shopLatitude");
        }
    }
}
