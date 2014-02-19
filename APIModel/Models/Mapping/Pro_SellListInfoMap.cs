using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellListInfoMap : EntityTypeConfiguration<Pro_SellListInfo>
    {
        public Pro_SellListInfoMap()
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

            this.Property(t => t.ClassType)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Pro_SellListInfo");
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
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.AnBu).HasColumnName("AnBu");
            this.Property(t => t.LieShou).HasColumnName("LieShou");
            this.Property(t => t.LieShouPrice).HasColumnName("LieShouPrice");
            this.Property(t => t.OtherOff).HasColumnName("OtherOff");
            this.Property(t => t.SellAduitID).HasColumnName("SellAduitID");
            this.Property(t => t.BackAduitID).HasColumnName("BackAduitID");
            this.Property(t => t.OffAduitListID).HasColumnName("OffAduitListID");
            this.Property(t => t.AnBuPrice).HasColumnName("AnBuPrice");
            this.Property(t => t.YanbaoModelPrice).HasColumnName("YanbaoModelPrice");
            this.Property(t => t.NeedAduit).HasColumnName("NeedAduit");
            this.Property(t => t.ClassType).HasColumnName("ClassType");
            this.Property(t => t.ProOffListID).HasColumnName("ProOffListID");
            this.Property(t => t.RulesShowToCus).HasColumnName("RulesShowToCus");
            this.Property(t => t.RulesUnShowToCus).HasColumnName("RulesUnShowToCus");
            this.Property(t => t.RulesGetBack).HasColumnName("RulesGetBack");
            this.Property(t => t.RulesUnGetBack).HasColumnName("RulesUnGetBack");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellAduit)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.AduidID);
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_SellBackInfo_Aduit)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.BackAduitID);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Pro_SellInfo_Aduit)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.SellAduitID);
            this.HasOptional(t => t.Pro_SellOffAduitInfoList)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.OffAduitListID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.SellType);
            this.HasOptional(t => t.Pro_SellListServiceInfo)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.ServiceInfo);
            this.HasOptional(t => t.Pro_SellSpecalOffList)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.SpecialID);
            this.HasOptional(t => t.Pro_SellTypeProduct)
                .WithMany(t => t.Pro_SellListInfo)
                .HasForeignKey(d => d.SellType_Pro_ID);

        }
    }
}
