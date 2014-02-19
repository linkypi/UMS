using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_RepairReturnListInfoMap : EntityTypeConfiguration<Pro_RepairReturnListInfo>
    {
        public Pro_RepairReturnListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RepairReturnListID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.OLD_IMEI)
                .HasMaxLength(50);

            this.Property(t => t.NEW_IMEI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_RepairReturnListInfo");
            this.Property(t => t.RepairReturnListID).HasColumnName("RepairReturnListID");
            this.Property(t => t.RepairReturnID).HasColumnName("RepairReturnID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.RepairListID).HasColumnName("RepairListID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.OLD_IMEI).HasColumnName("OLD_IMEI");
            this.Property(t => t.NEW_IMEI).HasColumnName("NEW_IMEI");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_RepairReturnListInfo)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_RepairListInfo)
                .WithMany(t => t.Pro_RepairReturnListInfo)
                .HasForeignKey(d => d.RepairListID);
            this.HasOptional(t => t.Pro_RepairReturnInfo)
                .WithMany(t => t.Pro_RepairReturnListInfo)
                .HasForeignKey(d => d.RepairReturnID);

        }
    }
}
