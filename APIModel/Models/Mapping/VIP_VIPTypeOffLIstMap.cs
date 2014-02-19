using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPTypeOffLIstMap : EntityTypeConfiguration<VIP_VIPTypeOffLIst>
    {
        public VIP_VIPTypeOffLIstMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPTypeOffLIst");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.TempOffID).HasColumnName("TempOffID");

            // Relationships
            this.HasRequired(t => t.VIP_OffList)
                .WithMany(t => t.VIP_VIPTypeOffLIst)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.VIP_OffListAduit)
                .WithMany(t => t.VIP_VIPTypeOffLIst)
                .HasForeignKey(d => d.TempOffID);
            this.HasOptional(t => t.VIP_VIPType)
                .WithMany(t => t.VIP_VIPTypeOffLIst)
                .HasForeignKey(d => d.VIPType);

        }
    }
}
