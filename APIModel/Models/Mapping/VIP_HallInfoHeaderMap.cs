using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_HallInfoHeaderMap : EntityTypeConfiguration<VIP_HallInfoHeader>
    {
        public VIP_HallInfoHeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_HallInfoHeader");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HeadID).HasColumnName("HeadID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.VIP_HallInfoHeader)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.VIP_OffListAduitHeader)
                .WithMany(t => t.VIP_HallInfoHeader)
                .HasForeignKey(d => d.HeadID);

        }
    }
}
