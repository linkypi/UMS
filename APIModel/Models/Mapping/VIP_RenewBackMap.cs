using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_RenewBackMap : EntityTypeConfiguration<VIP_RenewBack>
    {
        public VIP_RenewBackMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_RenewBack");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Old_Renew_ID).HasColumnName("Old_Renew_ID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.NewDate).HasColumnName("NewDate");

            // Relationships
            this.HasOptional(t => t.VIP_Renew)
                .WithMany(t => t.VIP_RenewBack)
                .HasForeignKey(d => d.Old_Renew_ID);
            this.HasOptional(t => t.VIP_RenewBackAduit)
                .WithMany(t => t.VIP_RenewBack)
                .HasForeignKey(d => d.AduitID);

        }
    }
}
