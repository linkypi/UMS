using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPTypeServiceMap : EntityTypeConfiguration<VIP_VIPTypeService>
    {
        public VIP_VIPTypeServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPTypeService");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.SCount).HasColumnName("SCount");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.VIP_VIPTypeService)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.VIP_VIPType)
                .WithMany(t => t.VIP_VIPTypeService)
                .HasForeignKey(d => d.TypeID);

        }
    }
}
