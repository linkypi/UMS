using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_PriceCostChangeMap : EntityTypeConfiguration<Pro_PriceCostChange>
    {
        public Pro_PriceCostChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ChangedID)
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_PriceCostChange");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ChangedID).HasColumnName("ChangedID");
        }
    }
}
