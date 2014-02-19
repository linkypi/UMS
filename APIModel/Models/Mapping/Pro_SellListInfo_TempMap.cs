using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellListInfo_TempMap : EntityTypeConfiguration<Pro_SellListInfo_Temp>
    {
        public Pro_SellListInfo_TempMap()
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

            this.Property(t => t.ChargePhoneNum)
                .HasMaxLength(20);

            this.Property(t => t.OldID)
                .HasMaxLength(100);

            this.Property(t => t.ChargePhoneName)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellListInfo_Temp");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
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
            this.Property(t => t.ServiceInfo).HasColumnName("ServiceInfo");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.AduidID).HasColumnName("AduidID");
            this.Property(t => t.AduidedOldPrice).HasColumnName("AduidedOldPrice");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.SpecialID).HasColumnName("SpecialID");
            this.Property(t => t.SellType_Pro_ID).HasColumnName("SellType_Pro_ID");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.OffSepecialPrice).HasColumnName("OffSepecialPrice");
            this.Property(t => t.WholeSaleOffPrice).HasColumnName("WholeSaleOffPrice");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.OldSellListID).HasColumnName("OldSellListID");
            this.Property(t => t.IsFree).HasColumnName("IsFree");
            this.Property(t => t.ChargePhoneNum).HasColumnName("ChargePhoneNum");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.ChargePhoneName).HasColumnName("ChargePhoneName");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.InsertDate).HasColumnName("InsertDate");
            this.Property(t => t.AnBu).HasColumnName("AnBu");
            this.Property(t => t.LieShouPrice).HasColumnName("LieShouPrice");
            this.Property(t => t.YanBaoModelPrice).HasColumnName("YanBaoModelPrice");
            this.Property(t => t.NeedAduit).HasColumnName("NeedAduit");
        }
    }
}
