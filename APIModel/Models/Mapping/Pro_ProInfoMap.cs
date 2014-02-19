using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ProInfoMap : EntityTypeConfiguration<Pro_ProInfo>
    {
        public Pro_ProInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProID);

            // Properties
            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ProFormat)
                .HasMaxLength(100);

            this.Property(t => t.PrintName)
                .HasMaxLength(8);

            this.Property(t => t.AirHallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ProInfo");
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
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.PrintName).HasColumnName("PrintName");
            this.Property(t => t.ISdecimals).HasColumnName("ISdecimals");
            this.Property(t => t.YanBaoModelID).HasColumnName("YanBaoModelID");
            this.Property(t => t.AirHallID).HasColumnName("AirHallID");
            this.Property(t => t.Pro_ClassTypeID).HasColumnName("Pro_ClassTypeID");

            // Relationships
            this.HasOptional(t => t.Pro_ClassInfo)
                .WithMany(t => t.Pro_ProInfo)
                .HasForeignKey(d => d.Pro_ClassID);
            this.HasOptional(t => t.Pro_ClassType)
                .WithMany(t => t.Pro_ProInfo)
                .HasForeignKey(d => d.Pro_ClassTypeID);
            this.HasOptional(t => t.Pro_TypeInfo)
                .WithMany(t => t.Pro_ProInfo)
                .HasForeignKey(d => d.Pro_TypeID);
            this.HasRequired(t => t.VIP_VIPType)
                .WithMany(t => t.Pro_ProInfo)
                .HasForeignKey(d => d.VIP_TypeID);
            this.HasOptional(t => t.Pro_ProMainInfo)
                .WithMany(t => t.Pro_ProInfo)
                .HasForeignKey(d => d.ProMainID);

        }
    }
}
