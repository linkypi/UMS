using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_RepairInfoMap : EntityTypeConfiguration<View_Pro_RepairInfo>
    {
        public View_Pro_RepairInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IsReturn, t.IsReceive, t.ID });

            // Properties
            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.IsReturn)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.IsReceive)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.RepairDate)
                .HasMaxLength(100);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.RepairID)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_RepairInfo");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.IsReceive).HasColumnName("IsReceive");
            this.Property(t => t.RepairDate).HasColumnName("RepairDate");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
    }
}
