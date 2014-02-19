using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_OutInfoMap : EntityTypeConfiguration<Pro_OutInfo>
    {
        public Pro_OutInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.FromHallID)
                .HasMaxLength(50);

            this.Property(t => t.Pro_HallID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.OutOrderID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.FromUserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ToUserID)
                .HasMaxLength(50);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_OutInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.FromHallID).HasColumnName("FromHallID");
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
            this.Property(t => t.ToDate).HasColumnName("ToDate");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.Audit).HasColumnName("Audit");
            this.Property(t => t.CancelDate).HasColumnName("CancelDate");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_OutInfo)
                .HasForeignKey(d => d.Pro_HallID);
            this.HasOptional(t => t.Pro_HallInfo1)
                .WithMany(t => t.Pro_OutInfo1)
                .HasForeignKey(d => d.FromHallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_OutInfo)
                .HasForeignKey(d => d.ToUserID);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_OutInfo1)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.Sys_UserInfo2)
                .WithMany(t => t.Pro_OutInfo2)
                .HasForeignKey(d => d.FromUserID);

        }
    }
}