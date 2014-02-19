using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPOffLIstMap : EntityTypeConfiguration<VIP_VIPOffLIst>
    {
        public VIP_VIPOffLIstMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPOffLIst");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.VIPID).HasColumnName("VIPID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.TempOffID).HasColumnName("TempOffID");

            // Relationships
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.VIP_VIPOffLIst)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.VIP_OffListAduit)
                .WithMany(t => t.VIP_VIPOffLIst)
                .HasForeignKey(d => d.TempOffID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_VIPOffLIst)
                .HasForeignKey(d => d.VIPID);

        }
    }
}
