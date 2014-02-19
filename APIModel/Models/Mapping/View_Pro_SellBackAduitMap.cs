using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_SellBackAduitMap : EntityTypeConfiguration<View_Pro_SellBackAduit>
    {
        public View_Pro_SellBackAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Aduited, t.Passed, t.Used, t.SID, t.HasUsed, t.HasAduited, t.HasPassed });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Used)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.UseDate)
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.AduitDate)
                .HasMaxLength(100);

            this.Property(t => t.AuditID)
                .HasMaxLength(50);

            this.Property(t => t.SellIDS)
                .HasMaxLength(50);

            this.Property(t => t.SellDate)
                .HasMaxLength(100);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.SellNote)
                .HasMaxLength(50);

            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_SellBackAduit");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitMoney).HasColumnName("AduitMoney");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.AuditID).HasColumnName("AuditID");
            this.Property(t => t.SellIDS).HasColumnName("SellIDS");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SellSysDate).HasColumnName("SellSysDate");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.SellNote).HasColumnName("SellNote");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.CardPay).HasColumnName("CardPay");
            this.Property(t => t.CashPay).HasColumnName("CashPay");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.SpecalOffID).HasColumnName("SpecalOffID");
            this.Property(t => t.OffTicketID).HasColumnName("OffTicketID");
            this.Property(t => t.OffTicketPrice).HasColumnName("OffTicketPrice");
            this.Property(t => t.CashTotle).HasColumnName("CashTotle");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.ApplyUser).HasColumnName("ApplyUser");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.AduitUser).HasColumnName("AduitUser");
            this.Property(t => t.ApplyMoney).HasColumnName("ApplyMoney");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.HasUsed).HasColumnName("HasUsed");
            this.Property(t => t.HasAduited).HasColumnName("HasAduited");
            this.Property(t => t.HasPassed).HasColumnName("HasPassed");
        }
    }
}
