using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackMap : EntityTypeConfiguration<Pro_SellBack>
    {
        public Pro_SellBackMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SellBackID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.BillID)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.CusVIPCardID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellBack");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellBackID).HasColumnName("SellBackID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.BackMoney).HasColumnName("BackMoney");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.OffTicketID).HasColumnName("OffTicketID");
            this.Property(t => t.OffTicketPrice).HasColumnName("OffTicketPrice");
            this.Property(t => t.CashTotle).HasColumnName("CashTotle");
            this.Property(t => t.BackOffTicketID).HasColumnName("BackOffTicketID");
            this.Property(t => t.BackOffTicketPrice).HasColumnName("BackOffTicketPrice");
            this.Property(t => t.CardPay).HasColumnName("CardPay");
            this.Property(t => t.CashPay).HasColumnName("CashPay");
            this.Property(t => t.OldCashTotle).HasColumnName("OldCashTotle");
            this.Property(t => t.BillID).HasColumnName("BillID");
            this.Property(t => t.ShouldBackCash).HasColumnName("ShouldBackCash");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.CusVIPCardID).HasColumnName("CusVIPCardID");
            this.Property(t => t.NewCashTotle).HasColumnName("NewCashTotle");

            // Relationships
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellBack)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellBack)
                .HasForeignKey(d => d.UpdUser);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellBack1)
                .HasForeignKey(d => d.UserID);

        }
    }
}
