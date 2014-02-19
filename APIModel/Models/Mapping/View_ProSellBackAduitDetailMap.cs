using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProSellBackAduitDetailMap : EntityTypeConfiguration<View_ProSellBackAduitDetail>
    {
        public View_ProSellBackAduitDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProCount, t.ProPrice, t.OffPrice, t.OffSepecialPrice, t.OtherCash, t.TicketUsed, t.BackCount, t.BackPrice, t.AduitBackPrice, t.CashTicket, t.CashPrice, t.SellListID });

            // Properties
            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OffPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OffSepecialPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OtherCash)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.TicketID)
                .HasMaxLength(50);

            this.Property(t => t.TicketUsed)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BackCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BackPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AduitBackPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.CashTicket)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CashPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellListID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ProSellBackAduitDetail");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.OffSepecialPrice).HasColumnName("OffSepecialPrice");
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.TicketID).HasColumnName("TicketID");
            this.Property(t => t.TicketUsed).HasColumnName("TicketUsed");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.BackCount).HasColumnName("BackCount");
            this.Property(t => t.BackPrice).HasColumnName("BackPrice");
            this.Property(t => t.AduitBackPrice).HasColumnName("AduitBackPrice");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.CashTicket).HasColumnName("CashTicket");
            this.Property(t => t.CashPrice).HasColumnName("CashPrice");
            this.Property(t => t.AnBu).HasColumnName("AnBu");
            this.Property(t => t.LieShouPrice).HasColumnName("LieShouPrice");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.HallID).HasColumnName("HallID");
        }
    }
}
