using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellInfo_AduitMap : EntityTypeConfiguration<Pro_SellInfo_Aduit>
    {
        public Pro_SellInfo_AduitMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SellID)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.AuditID)
                .HasMaxLength(50);

            this.Property(t => t.BillID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellInfo_Aduit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.HallID).HasColumnName("HallID");
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
            this.Property(t => t.AuditID).HasColumnName("AuditID");
            this.Property(t => t.BillID).HasColumnName("BillID");
            this.Property(t => t.OffAduit).HasColumnName("OffAduit");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Pro_SellOffAduitInfo)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.OffAduit);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.VIP_OffTicket)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.OffTicketID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.Seller);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellInfo_Aduit1)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.Pro_SellInfo_Aduit)
                .HasForeignKey(d => d.VIP_ID);

        }
    }
}
