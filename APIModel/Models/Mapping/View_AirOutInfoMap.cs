using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_AirOutInfoMap : EntityTypeConfiguration<View_AirOutInfo>
    {
        public View_AirOutInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Audit, t.ID });

            // Properties
            this.Property(t => t.FromHallName)
                .HasMaxLength(50);

            this.Property(t => t.ToHallName)
                .HasMaxLength(50);

            this.Property(t => t.Audit)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FromUserName)
                .HasMaxLength(50);

            this.Property(t => t.ToUserName)
                .HasMaxLength(50);

            this.Property(t => t.FromHallID)
                .HasMaxLength(50);

            this.Property(t => t.Pro_HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.OutOrderID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.OutTime)
                .HasMaxLength(100);

            this.Property(t => t.NewToDate)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("View_AirOutInfo");
            this.Property(t => t.FromHallName).HasColumnName("FromHallName");
            this.Property(t => t.ToHallName).HasColumnName("ToHallName");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.ToDate).HasColumnName("ToDate");
            this.Property(t => t.Audit).HasColumnName("Audit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.FromUserName).HasColumnName("FromUserName");
            this.Property(t => t.ToUserName).HasColumnName("ToUserName");
            this.Property(t => t.FromHallID).HasColumnName("FromHallID");
            this.Property(t => t.Pro_HallID).HasColumnName("Pro_HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.OutOrderID).HasColumnName("OutOrderID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.OutDate).HasColumnName("OutDate");
            this.Property(t => t.OutTime).HasColumnName("OutTime");
            this.Property(t => t.NewToDate).HasColumnName("NewToDate");
        }
    }
}
