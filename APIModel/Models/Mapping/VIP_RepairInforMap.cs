using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_RepairInforMap : EntityTypeConfiguration<VIP_RepairInfor>
    {
        public VIP_RepairInforMap()
        {
            // Primary Key
            this.HasKey(t => t.repairInforId);

            // Properties
            this.Property(t => t.servicePoint)
                .HasMaxLength(100);

            this.Property(t => t.oddNum)
                .HasMaxLength(50);

            this.Property(t => t.progress)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.productName)
                .HasMaxLength(100);

            this.Property(t => t.productColour)
                .HasMaxLength(50);

            this.Property(t => t.productSeries)
                .HasMaxLength(50);

            this.Property(t => t.productNo)
                .HasMaxLength(100);

            this.Property(t => t.producRemarks)
                .HasMaxLength(250);

            this.Property(t => t.phoneAccessory)
                .HasMaxLength(150);

            this.Property(t => t.manualOddNum)
                .HasMaxLength(150);

            this.Property(t => t.repairInforWay)
                .HasMaxLength(150);

            this.Property(t => t.receivingMan)
                .HasMaxLength(50);

            this.Property(t => t.engineer)
                .HasMaxLength(50);

            this.Property(t => t.vipImei)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_RepairInfor");
            this.Property(t => t.repairInforId).HasColumnName("repairInforId");
            this.Property(t => t.servicePoint).HasColumnName("servicePoint");
            this.Property(t => t.oddNum).HasColumnName("oddNum");
            this.Property(t => t.progress).HasColumnName("progress");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.productName).HasColumnName("productName");
            this.Property(t => t.productColour).HasColumnName("productColour");
            this.Property(t => t.productSeries).HasColumnName("productSeries");
            this.Property(t => t.productNo).HasColumnName("productNo");
            this.Property(t => t.producRemarks).HasColumnName("producRemarks");
            this.Property(t => t.phoneAccessory).HasColumnName("phoneAccessory");
            this.Property(t => t.manualOddNum).HasColumnName("manualOddNum");
            this.Property(t => t.repairInforWay).HasColumnName("repairInforWay");
            this.Property(t => t.receivingMan).HasColumnName("receivingMan");
            this.Property(t => t.engineer).HasColumnName("engineer");
            this.Property(t => t.repairInforTime).HasColumnName("repairInforTime");
            this.Property(t => t.vipImei).HasColumnName("vipImei");
            this.Property(t => t.progressid).HasColumnName("progressid");
        }
    }
}
