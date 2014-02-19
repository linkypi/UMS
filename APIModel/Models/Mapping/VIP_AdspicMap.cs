using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_AdspicMap : EntityTypeConfiguration<VIP_Adspic>
    {
        public VIP_AdspicMap()
        {
            // Primary Key
            this.HasKey(t => t.adsPicsid);

            // Properties
            this.Property(t => t.adsPics)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_Adspic");
            this.Property(t => t.adsPicsid).HasColumnName("adsPicsid");
            this.Property(t => t.adsId).HasColumnName("adsId");
            this.Property(t => t.adsPics).HasColumnName("adsPics");
        }
    }
}
