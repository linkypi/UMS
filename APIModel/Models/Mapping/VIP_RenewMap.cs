using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_RenewMap : EntityTypeConfiguration<VIP_Renew>
    {
        public VIP_RenewMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RenewTypeName)
                .HasMaxLength(50);

            this.Property(t => t.RenewTypeClassName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_Renew");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.RenewMoney).HasColumnName("RenewMoney");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.RenewTypeName).HasColumnName("RenewTypeName");
            this.Property(t => t.RenewTypeClassName).HasColumnName("RenewTypeClassName");
            this.Property(t => t.RenewValue1).HasColumnName("RenewValue1");
            this.Property(t => t.RenewValue2).HasColumnName("RenewValue2");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.RenewDate).HasColumnName("RenewDate");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.OldEndDate).HasColumnName("OldEndDate");
            this.Property(t => t.Seller).HasColumnName("Seller");

            // Relationships
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_Renew)
                .HasForeignKey(d => d.VIP_ID);

        }
    }
}
