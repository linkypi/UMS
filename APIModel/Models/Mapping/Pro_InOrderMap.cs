using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_InOrderMap : EntityTypeConfiguration<Pro_InOrder>
    {
        public Pro_InOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.InOrderID)
                .HasMaxLength(50);

            this.Property(t => t.Pro_HallID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_InOrder");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.InOrderID).HasColumnName("InOrderID");
            this.Property(t => t.Pro_HallID).HasColumnName("Pro_HallID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.InDate).HasColumnName("InDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_InOrder)
                .HasForeignKey(d => d.Pro_HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_InOrder)
                .HasForeignKey(d => d.UserID);

        }
    }
}
