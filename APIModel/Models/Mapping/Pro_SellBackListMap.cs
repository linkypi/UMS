using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackListMap : EntityTypeConfiguration<Pro_SellBackList>
    {
        public Pro_SellBackListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.TicketID)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellBackList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.CashTicket).HasColumnName("CashTicket");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.TicketID).HasColumnName("TicketID");
            this.Property(t => t.TicketUsed).HasColumnName("TicketUsed");
            this.Property(t => t.CashPrice).HasColumnName("CashPrice");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.AduidedNewPrice).HasColumnName("AduidedNewPrice");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.OffSepecialPrice).HasColumnName("OffSepecialPrice");
            this.Property(t => t.WholeSaleOffPrice).HasColumnName("WholeSaleOffPrice");
            this.Property(t => t.SellType_Pro_ID).HasColumnName("SellType_Pro_ID");
            this.Property(t => t.SpecialID).HasColumnName("SpecialID");
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.ShouldBackCash).HasColumnName("ShouldBackCash");
            this.Property(t => t.AnBu).HasColumnName("AnBu");
            this.Property(t => t.LieShou).HasColumnName("LieShou");
            this.Property(t => t.LieShouPrice).HasColumnName("LieShouPrice");
            this.Property(t => t.OtherOff).HasColumnName("OtherOff");
            this.Property(t => t.AnBuPrice).HasColumnName("AnBuPrice");
            this.Property(t => t.BackAduitID).HasColumnName("BackAduitID");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_SellBackInfo_Aduit)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.BackAduitID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_SellBackList)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
