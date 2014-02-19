using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_HallOffInfoMap : EntityTypeConfiguration<VIP_HallOffInfo>
    {
        public VIP_HallOffInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_HallOffInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.TempOffID).HasColumnName("TempOffID");

            // Relationships
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.VIP_HallOffInfo)
                .HasForeignKey(d => d.OffID);

        }
    }
}
