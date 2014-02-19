using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_RepairListInfoMap : EntityTypeConfiguration<Pro_RepairListInfo>
    {
        public Pro_RepairListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RepairListID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_RepairListInfo");
            this.Property(t => t.RepairListID).HasColumnName("RepairListID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.IMEI).HasColumnName("IMEI");

            // Relationships
            this.HasOptional(t => t.Pro_RepairInfo)
                .WithMany(t => t.Pro_RepairListInfo)
                .HasForeignKey(d => d.RepairID);

        }
    }
}
