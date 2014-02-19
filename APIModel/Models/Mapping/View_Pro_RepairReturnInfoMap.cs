using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_RepairReturnInfoMap : EntityTypeConfiguration<View_Pro_RepairReturnInfo>
    {
        public View_Pro_RepairReturnInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Canceled, t.IsReceived });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.RepairReturnDate)
                .HasMaxLength(100);

            this.Property(t => t.RepairReturnID)
                .HasMaxLength(50);

            this.Property(t => t.Canceled)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.IsReceived)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_RepairReturnInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.RepairReturnDate).HasColumnName("RepairReturnDate");
            this.Property(t => t.RepairReturnID).HasColumnName("RepairReturnID");
            this.Property(t => t.Canceled).HasColumnName("Canceled");
            this.Property(t => t.IsReceived).HasColumnName("IsReceived");
            this.Property(t => t.OldID).HasColumnName("OldID");
        }
    }
}
