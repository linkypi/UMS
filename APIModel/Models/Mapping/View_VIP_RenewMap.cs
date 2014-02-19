using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIP_RenewMap : EntityTypeConfiguration<View_VIP_Renew>
    {
        public View_VIP_RenewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RenewID, t.State });

            // Properties
            this.Property(t => t.MemberName)
                .HasMaxLength(50);

            this.Property(t => t.Sex)
                .HasMaxLength(10);

            this.Property(t => t.MobiPhone)
                .HasMaxLength(50);

            this.Property(t => t.TelePhone)
                .HasMaxLength(50);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.StartTime)
                .HasMaxLength(100);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.CardTypeName)
                .HasMaxLength(50);

            this.Property(t => t.RenewTypeName)
                .HasMaxLength(50);

            this.Property(t => t.RenewTypeClassName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.RenewDate)
                .HasMaxLength(100);

            this.Property(t => t.RenewID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.State)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.VIPType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_VIP_Renew");
            this.Property(t => t.VIPID).HasColumnName("VIPID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.MobiPhone).HasColumnName("MobiPhone");
            this.Property(t => t.TelePhone).HasColumnName("TelePhone");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.CurrentValidity).HasColumnName("CurrentValidity");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.CurrentPoint).HasColumnName("CurrentPoint");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.CardTypeName).HasColumnName("CardTypeName");
            this.Property(t => t.IDCard_ID).HasColumnName("IDCard_ID");
            this.Property(t => t.RenewMoney).HasColumnName("RenewMoney");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.RenewTypeName).HasColumnName("RenewTypeName");
            this.Property(t => t.RenewTypeClassName).HasColumnName("RenewTypeClassName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.RenewDate).HasColumnName("RenewDate");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.RenewValue1).HasColumnName("RenewValue1");
            this.Property(t => t.RenewValue2).HasColumnName("RenewValue2");
            this.Property(t => t.RenewID).HasColumnName("RenewID");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
        }
    }
}
