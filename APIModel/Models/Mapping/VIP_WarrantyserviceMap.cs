using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_WarrantyserviceMap : EntityTypeConfiguration<VIP_Warrantyservice>
    {
        public VIP_WarrantyserviceMap()
        {
            // Primary Key
            this.HasKey(t => t.warrantyId);

            // Properties
            this.Property(t => t.customerName)
                .HasMaxLength(50);

            this.Property(t => t.IDCard)
                .HasMaxLength(150);

            this.Property(t => t.phone)
                .HasMaxLength(50);

            this.Property(t => t.phonePrice)
                .HasMaxLength(50);

            this.Property(t => t.warrantyPrice)
                .HasMaxLength(50);

            this.Property(t => t.phoneModel)
                .HasMaxLength(50);

            this.Property(t => t.phoneImei)
                .HasMaxLength(50);

            this.Property(t => t.tickeNum)
                .HasMaxLength(50);

            this.Property(t => t.batteryNum)
                .HasMaxLength(50);

            this.Property(t => t.chargerNum)
                .HasMaxLength(50);

            this.Property(t => t.agreementNum)
                .HasMaxLength(80);

            this.Property(t => t.vipImei)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_Warrantyservice");
            this.Property(t => t.warrantyId).HasColumnName("warrantyId");
            this.Property(t => t.customerName).HasColumnName("customerName");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.phone).HasColumnName("phone");
            this.Property(t => t.phonePrice).HasColumnName("phonePrice");
            this.Property(t => t.warrantyPrice).HasColumnName("warrantyPrice");
            this.Property(t => t.warrantyStTime).HasColumnName("warrantyStTime");
            this.Property(t => t.warrantyEdTime).HasColumnName("warrantyEdTime");
            this.Property(t => t.phoneModel).HasColumnName("phoneModel");
            this.Property(t => t.phoneImei).HasColumnName("phoneImei");
            this.Property(t => t.tickeNum).HasColumnName("tickeNum");
            this.Property(t => t.batteryNum).HasColumnName("batteryNum");
            this.Property(t => t.chargerNum).HasColumnName("chargerNum");
            this.Property(t => t.agreementNum).HasColumnName("agreementNum");
            this.Property(t => t.vipImei).HasColumnName("vipImei");
            this.Property(t => t.servePhone).HasColumnName("servePhone");
        }
    }
}
