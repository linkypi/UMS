using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_RepairReturnInfoMap : EntityTypeConfiguration<Pro_RepairReturnInfo>
    {
        public Pro_RepairReturnInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.RepairReturnID)
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
            this.ToTable("Pro_RepairReturnInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.RepairReturnID).HasColumnName("RepairReturnID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.RepairReturnDate).HasColumnName("RepairReturnDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.IsReceived).HasColumnName("IsReceived");
            this.Property(t => t.Receiver).HasColumnName("Receiver");
            this.Property(t => t.RecvTime).HasColumnName("RecvTime");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_RepairReturnInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Pro_RepairInfo)
                .WithMany(t => t.Pro_RepairReturnInfo)
                .HasForeignKey(d => d.RepairID);
            this.HasRequired(t => t.Pro_RepairReturnInfo2)
                .WithOptional(t => t.Pro_RepairReturnInfo1);

        }
    }
}
