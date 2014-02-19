using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Sell_Back_NewSpecialOffListInfoMap : EntityTypeConfiguration<View_Sell_Back_NewSpecialOffListInfo>
    {
        public View_Sell_Back_NewSpecialOffListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ClassName, t.typename, t.proname, t.proformat, t.procount, t.selltypename, t.imei, t.ticketid, t.offname, t.isfree, t.ProID, t.NOte });

            // Properties
            this.Property(t => t.SellID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.typename)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.proname)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.proformat)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.procount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.selltypename)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.imei)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ticketid)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.offname)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.sepecialoffname)
                .HasMaxLength(50);

            this.Property(t => t.isfree)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.hallid)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.NOte)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("View_Sell_Back_NewSpecialOffListInfo");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.Pro_ClassID).HasColumnName("Pro_ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Pro_typeid).HasColumnName("Pro_typeid");
            this.Property(t => t.typename).HasColumnName("typename");
            this.Property(t => t.proname).HasColumnName("proname");
            this.Property(t => t.proformat).HasColumnName("proformat");
            this.Property(t => t.procount).HasColumnName("procount");
            this.Property(t => t.sellType).HasColumnName("sellType");
            this.Property(t => t.selltypename).HasColumnName("selltypename");
            this.Property(t => t.proprice).HasColumnName("proprice");
            this.Property(t => t.RealPrice).HasColumnName("RealPrice");
            this.Property(t => t.anbu).HasColumnName("anbu");
            this.Property(t => t.AnBuPrice).HasColumnName("AnBuPrice");
            this.Property(t => t.imei).HasColumnName("imei");
            this.Property(t => t.ticketid).HasColumnName("ticketid");
            this.Property(t => t.cashticket).HasColumnName("cashticket");
            this.Property(t => t.ticketused).HasColumnName("ticketused");
            this.Property(t => t.offpoint).HasColumnName("offpoint");
            this.Property(t => t.offname).HasColumnName("offname");
            this.Property(t => t.offprice).HasColumnName("offprice");
            this.Property(t => t.sepecialoffname).HasColumnName("sepecialoffname");
            this.Property(t => t.offsepecialprice).HasColumnName("offsepecialprice");
            this.Property(t => t.wholesaleoffprice).HasColumnName("wholesaleoffprice");
            this.Property(t => t.OtherOff).HasColumnName("OtherOff");
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.LieShouPrice).HasColumnName("LieShouPrice");
            this.Property(t => t.isfree).HasColumnName("isfree");
            this.Property(t => t.cashprice).HasColumnName("cashprice");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.hallid).HasColumnName("hallid");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.Vip_ID).HasColumnName("Vip_ID");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.NOte).HasColumnName("NOte");
            this.Property(t => t.sellDate).HasColumnName("sellDate");
        }
    }
}
