using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_IMEIMap : EntityTypeConfiguration<Pro_IMEI>
    {
        public Pro_IMEIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_IMEI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.NEW_IMEI_ID).HasColumnName("NEW_IMEI_ID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.OutID).HasColumnName("OutID");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.ReturnID).HasColumnName("ReturnID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.VIPID).HasColumnName("VIPID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.AuditID).HasColumnName("AuditID");

            // Relationships
            this.HasOptional(t => t.Pro_BorowOrderIMEI)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.BorowID);
            this.HasOptional(t => t.Pro_SellOffAduitInfo)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.AuditID);
            this.HasRequired(t => t.Pro_IMEI2)
                .WithOptional(t => t.Pro_IMEI1);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Pro_IMEI3)
                .WithMany(t => t.Pro_IMEI11)
                .HasForeignKey(d => d.NEW_IMEI_ID);
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_OutInfo)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.OutID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_RepairListInfo)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.RepairID);
            this.HasOptional(t => t.Pro_ReturnOrderIMEI)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.ReturnID);
            this.HasOptional(t => t.VIP_VIPInfo_Temp)
                .WithMany(t => t.Pro_IMEI)
                .HasForeignKey(d => d.VIPID);

        }
    }
}
