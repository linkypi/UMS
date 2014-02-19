using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SMS_SignInfoMap : EntityTypeConfiguration<View_SMS_SignInfo>
    {
        public View_SMS_SignInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellID)
                .HasMaxLength(50);

            this.Property(t => t.OldSellID)
                .HasMaxLength(50);

            this.Property(t => t.username)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Industry)
                .HasMaxLength(50);

            this.Property(t => t.SignDate)
                .HasMaxLength(50);

            this.Property(t => t.CpcName)
                .HasMaxLength(50);

            this.Property(t => t.CpcAdd)
                .HasMaxLength(500);

            this.Property(t => t.SMSContent)
                .HasMaxLength(50);

            this.Property(t => t.PayAllDate)
                .HasMaxLength(50);

            this.Property(t => t.RealPayAllDate)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.BillHeader)
                .HasMaxLength(50);

            this.Property(t => t.BillNum)
                .HasMaxLength(50);

            this.Property(t => t.BillDate)
                .HasMaxLength(50);

            this.Property(t => t.Sellor)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SMS_SignInfo");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.OldSellID).HasColumnName("OldSellID");
            this.Property(t => t.username).HasColumnName("username");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Industry).HasColumnName("Industry");
            this.Property(t => t.SignDate).HasColumnName("SignDate");
            this.Property(t => t.CpcName).HasColumnName("CpcName");
            this.Property(t => t.CpcAdd).HasColumnName("CpcAdd");
            this.Property(t => t.SMSContent).HasColumnName("SMSContent");
            this.Property(t => t.SignPay).HasColumnName("SignPay");
            this.Property(t => t.SignCount).HasColumnName("SignCount");
            this.Property(t => t.RealPay).HasColumnName("RealPay");
            this.Property(t => t.RealCount).HasColumnName("RealCount");
            this.Property(t => t.PayAllDate).HasColumnName("PayAllDate");
            this.Property(t => t.RealPayAllDate).HasColumnName("RealPayAllDate");
            this.Property(t => t.PayBack).HasColumnName("PayBack");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.BillHeader).HasColumnName("BillHeader");
            this.Property(t => t.BillNum).HasColumnName("BillNum");
            this.Property(t => t.BillDate).HasColumnName("BillDate");
            this.Property(t => t.Sellor).HasColumnName("Sellor");
            this.Property(t => t.RatePay).HasColumnName("RatePay");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsOver).HasColumnName("IsOver");
        }
    }
}
