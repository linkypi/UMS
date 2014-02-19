using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SpecialOffListInfoMap : EntityTypeConfiguration<View_SpecialOffListInfo>
    {
        public View_SpecialOffListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.OldID, t.classname, t.typename, t.proname, t.proformat, t.ProCount, t.SellTypeName, t.IMEI, t.TicketID, t.OffName, t.IsFree, t.selldate });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OldID)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.classname)
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

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellTypeName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.IMEI)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.TicketID)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.OffName)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.SepecialOffName)
                .HasMaxLength(50);

            this.Property(t => t.IsFree)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.selldate)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("View_SpecialOffListInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.pro_classid).HasColumnName("pro_classid");
            this.Property(t => t.classname).HasColumnName("classname");
            this.Property(t => t.pro_typeid).HasColumnName("pro_typeid");
            this.Property(t => t.typename).HasColumnName("typename");
            this.Property(t => t.proname).HasColumnName("proname");
            this.Property(t => t.proformat).HasColumnName("proformat");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.selltype).HasColumnName("selltype");
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
            this.Property(t => t.OtherCash).HasColumnName("OtherCash");
            this.Property(t => t.IsFree).HasColumnName("IsFree");
            this.Property(t => t.CashPrice).HasColumnName("CashPrice");
            this.Property(t => t.selldate).HasColumnName("selldate");
            this.Property(t => t.BackID).HasColumnName("BackID");
        }
    }
}
