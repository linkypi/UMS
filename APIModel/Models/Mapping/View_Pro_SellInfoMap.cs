using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_SellInfoMap : EntityTypeConfiguration<View_Pro_SellInfo>
    {
        public View_Pro_SellInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Applyed });

            // Properties
            this.Property(t => t.AuditID)
                .HasMaxLength(50);

            this.Property(t => t.SellID)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.SellDate)
                .HasMaxLength(100);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Applyed)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("View_Pro_SellInfo");
            this.Property(t => t.AuditID).HasColumnName("AuditID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.CardPay).HasColumnName("CardPay");
            this.Property(t => t.CashPay).HasColumnName("CashPay");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.SpecalOffID).HasColumnName("SpecalOffID");
            this.Property(t => t.OffTicketID).HasColumnName("OffTicketID");
            this.Property(t => t.OffTicketPrice).HasColumnName("OffTicketPrice");
            this.Property(t => t.CashTotle).HasColumnName("CashTotle");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Applyed).HasColumnName("Applyed");
        }
    }
}
