using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_SendProOffListMap : EntityTypeConfiguration<VIP_SendProOffList>
    {
        public VIP_SendProOffListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_SendProOffList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.LimitCount).HasColumnName("LimitCount");
            this.Property(t => t.PerCount).HasColumnName("PerCount");
            this.Property(t => t.ProCost).HasColumnName("ProCost");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.VIP_SendProOffList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.VIP_SendProOffList)
                .HasForeignKey(d => d.OffID);

        }
    }
}
