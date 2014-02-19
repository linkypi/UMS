using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_InOrderMap : EntityTypeConfiguration<View_Pro_InOrder>
    {
        public View_Pro_InOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_InOrder");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.InOrderID).HasColumnName("InOrderID");
            this.Property(t => t.Pro_HallID).HasColumnName("Pro_HallID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.InDate).HasColumnName("InDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.HallName).HasColumnName("HallName");
        }
    }
}
