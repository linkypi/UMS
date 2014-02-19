using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_OutSearchMap : EntityTypeConfiguration<View_OutSearch>
    {
        public View_OutSearchMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Aduit });

            // Properties
            this.Property(t => t.FromHallID)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Pro_HallID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.OutOrderID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.OutDate)
                .HasMaxLength(100);

            this.Property(t => t.FromUserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ToUserID)
                .HasMaxLength(50);

            this.Property(t => t.NewToDate)
                .HasMaxLength(100);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            this.Property(t => t.Aduit)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.FromHallName)
                .HasMaxLength(50);

            this.Property(t => t.FromUserName)
                .HasMaxLength(50);

            this.Property(t => t.ToUserName)
                .HasMaxLength(50);

            this.Property(t => t.Pro_HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.DeleterName)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_OutSearch");
            this.Property(t => t.FromHallID).HasColumnName("FromHallID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Pro_HallID).HasColumnName("Pro_HallID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.OutOrderID).HasColumnName("OutOrderID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.OutDate).HasColumnName("OutDate");
            this.Property(t => t.FromUserID).HasColumnName("FromUserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ToUserID).HasColumnName("ToUserID");
            this.Property(t => t.NewToDate).HasColumnName("NewToDate");
            this.Property(t => t.ToDate).HasColumnName("ToDate");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.Aduit).HasColumnName("Aduit");
            this.Property(t => t.CancelDate).HasColumnName("CancelDate");
            this.Property(t => t.FromHallName).HasColumnName("FromHallName");
            this.Property(t => t.FromUserName).HasColumnName("FromUserName");
            this.Property(t => t.ToUserName).HasColumnName("ToUserName");
            this.Property(t => t.Pro_HallName).HasColumnName("Pro_HallName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DeleterName).HasColumnName("DeleterName");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
        }
    }
}
