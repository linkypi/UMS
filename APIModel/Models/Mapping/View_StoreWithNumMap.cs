using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_StoreWithNumMap : EntityTypeConfiguration<View_StoreWithNum>
    {
        public View_StoreWithNumMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Num, t.ProID, t.VIP_TypeID, t.NeedIMEI, t.BeforeSep, t.BeforeRate, t.AfterSep, t.AfterRate, t.TicketLevel, t.BeforeTicket, t.AfterTicket, t.IsService });

            // Properties
            this.Property(t => t.Num)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.AreaName)
                .HasMaxLength(50);

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

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_StoreWithNum");
            this.Property(t => t.Num).HasColumnName("Num");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
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
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
        }
    }
}
