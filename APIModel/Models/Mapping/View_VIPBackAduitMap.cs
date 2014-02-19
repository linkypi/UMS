using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPBackAduitMap : EntityTypeConfiguration<View_VIPBackAduit>
    {
        public View_VIPBackAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AduitID, t.ID, t.Aduited, t.Used });

            // Properties
            this.Property(t => t.AduitID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AduitDate)
                .HasMaxLength(100);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Used)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.UseDate)
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.MemberName)
                .HasMaxLength(50);

            this.Property(t => t.MobiPhone)
                .HasMaxLength(50);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.VIPType)
                .HasMaxLength(50);

            this.Property(t => t.CardTypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_VIPBackAduit");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.AduitUser).HasColumnName("AduitUser");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.ApplyUser).HasColumnName("ApplyUser");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.MobiPhone).HasColumnName("MobiPhone");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.CardTypeName).HasColumnName("CardTypeName");
        }
    }
}
