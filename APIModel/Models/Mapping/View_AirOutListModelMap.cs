using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_AirOutListModelMap : EntityTypeConfiguration<View_AirOutListModel>
    {
        public View_AirOutListModelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProCount, t.Audit, t.ID });

            // Properties
            this.Property(t => t.FromHallName)
                .HasMaxLength(50);

            this.Property(t => t.ToHallName)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.NewInListID)
                .HasMaxLength(50);

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NewProName)
                .HasMaxLength(50);

            this.Property(t => t.NewTypeName)
                .HasMaxLength(50);

            this.Property(t => t.NewClassName)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
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

            this.Property(t => t.FromUserID)
                .HasMaxLength(50);

            this.Property(t => t.ToUserID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_AirOutListModel");
            this.Property(t => t.FromHallName).HasColumnName("FromHallName");
            this.Property(t => t.ToHallName).HasColumnName("ToHallName");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.NewInListID).HasColumnName("NewInListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.NewProName).HasColumnName("NewProName");
            this.Property(t => t.NewTypeName).HasColumnName("NewTypeName");
            this.Property(t => t.NewClassName).HasColumnName("NewClassName");
            this.Property(t => t.NewProFormat).HasColumnName("NewProFormat");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
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
            this.Property(t => t.FromUserID).HasColumnName("FromUserID");
            this.Property(t => t.ToUserID).HasColumnName("ToUserID");
        }
    }
}
