using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SellBackListInfoMap : EntityTypeConfiguration<View_SellBackListInfo>
    {
        public View_SellBackListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SepecialOffName, t.IsFree });

            // Properties
            this.Property(t => t.SellBackID)
                .HasMaxLength(50);

            this.Property(t => t.BillID)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.TicketID)
                .HasMaxLength(50);

            this.Property(t => t.OffName)
                .HasMaxLength(50);

            this.Property(t => t.SepecialOffName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.IsFree)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ClassTypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SellBackListInfo");
            this.Property(t => t.SellBackID).HasColumnName("SellBackID");
            this.Property(t => t.BillID).HasColumnName("BillID");
            this.Property(t => t.Pro_ClassID).HasColumnName("Pro_ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Pro_TypeID).HasColumnName("Pro_TypeID");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.proprice).HasColumnName("proprice");
            this.Property(t => t.Realproprice).HasColumnName("Realproprice");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.TicketID).HasColumnName("TicketID");
            this.Property(t => t.CashTicket).HasColumnName("CashTicket");
            this.Property(t => t.TicketUsed).HasColumnName("TicketUsed");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.OffName).HasColumnName("OffName");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.SepecialOffName).HasColumnName("SepecialOffName");
            this.Property(t => t.OffSepecialPrice).HasColumnName("OffSepecialPrice");
            this.Property(t => t.WholeSaleOffPrice).HasColumnName("WholeSaleOffPrice");
            this.Property(t => t.otheroff).HasColumnName("otheroff");
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.IsFree).HasColumnName("IsFree");
            this.Property(t => t.CashPrice).HasColumnName("CashPrice");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.anbu).HasColumnName("anbu");
            this.Property(t => t.lieshou).HasColumnName("lieshou");
            this.Property(t => t.daixiaofei).HasColumnName("daixiaofei");
            this.Property(t => t.ClassTypeID).HasColumnName("ClassTypeID");
            this.Property(t => t.ClassTypeName).HasColumnName("ClassTypeName");
        }
    }
}
