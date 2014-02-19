using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_AdsMap : EntityTypeConfiguration<VIP_Ads>
    {
        public VIP_AdsMap()
        {
            // Primary Key
            this.HasKey(t => t.adsId);

            // Properties
            this.Property(t => t.adsName)
                .HasMaxLength(50);

            this.Property(t => t.adsInfo)
                .HasMaxLength(150);

            this.Property(t => t.adsPicbig)
                .HasMaxLength(50);

            this.Property(t => t.adsPicsid)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_Ads");
            this.Property(t => t.adsId).HasColumnName("adsId");
            this.Property(t => t.adsName).HasColumnName("adsName");
            this.Property(t => t.adsInfo).HasColumnName("adsInfo");
            this.Property(t => t.adsPicbig).HasColumnName("adsPicbig");
            this.Property(t => t.adsPicsid).HasColumnName("adsPicsid");
            this.Property(t => t.adscreatetime).HasColumnName("adscreatetime");
        }
    }
}
