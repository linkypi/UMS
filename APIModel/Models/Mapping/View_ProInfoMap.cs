using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProInfoMap : EntityTypeConfiguration<View_ProInfo>
    {
        public View_ProInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProID, t.VIP_TypeID, t.NeedIMEI, t.BeforeSep, t.BeforeRate, t.AfterSep, t.AfterRate, t.TicketLevel, t.BeforeTicket, t.AfterTicket, t.IsService, t.HasNeedIMEI, t.HasNeedMoreorLess, t.HasService, t.HasDecimals, t.ISdecimals });

            // Properties
            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.VIP_TypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.BeforeRate)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AfterRate)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TicketLevel)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BeforeTicket)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AfterTicket)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.HasNeedIMEI)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.HasNeedMoreorLess)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.HasService)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.HasDecimals)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.PrintName)
                .HasMaxLength(8);

            this.Property(t => t.AirHallID)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ProInfo");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.Pro_TypeID).HasColumnName("Pro_TypeID");
            this.Property(t => t.Pro_ClassID).HasColumnName("Pro_ClassID");
            this.Property(t => t.VIP_TypeID).HasColumnName("VIP_TypeID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.NeedIMEI).HasColumnName("NeedIMEI");
            this.Property(t => t.SepDate).HasColumnName("SepDate");
            this.Property(t => t.BeforeSep).HasColumnName("BeforeSep");
            this.Property(t => t.BeforeRate).HasColumnName("BeforeRate");
            this.Property(t => t.AfterSep).HasColumnName("AfterSep");
            this.Property(t => t.AfterRate).HasColumnName("AfterRate");
            this.Property(t => t.TicketLevel).HasColumnName("TicketLevel");
            this.Property(t => t.BeforeTicket).HasColumnName("BeforeTicket");
            this.Property(t => t.AfterTicket).HasColumnName("AfterTicket");
            this.Property(t => t.ValueIDList).HasColumnName("ValueIDList");
            this.Property(t => t.IsService).HasColumnName("IsService");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.NeedMoreorLess).HasColumnName("NeedMoreorLess");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.HasNeedIMEI).HasColumnName("HasNeedIMEI");
            this.Property(t => t.HasNeedMoreorLess).HasColumnName("HasNeedMoreorLess");
            this.Property(t => t.HasService).HasColumnName("HasService");
            this.Property(t => t.HasDecimals).HasColumnName("HasDecimals");
            this.Property(t => t.ISdecimals).HasColumnName("ISdecimals");
            this.Property(t => t.PrintName).HasColumnName("PrintName");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.YanBaoModelID).HasColumnName("YanBaoModelID");
            this.Property(t => t.AirHallID).HasColumnName("AirHallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
        }
    }
}
