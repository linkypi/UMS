using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_RepairInfoMap : EntityTypeConfiguration<Pro_RepairInfo>
    {
        public Pro_RepairInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.RepairID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            this.Property(t => t.Receiver)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_RepairInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.RepairDate).HasColumnName("RepairDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.IsReceive).HasColumnName("IsReceive");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.Receiver).HasColumnName("Receiver");
            this.Property(t => t.RecvTime).HasColumnName("RecvTime");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_RepairInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_RepairInfo)
                .HasForeignKey(d => d.UserID);

        }
    }
}
