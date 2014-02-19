using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_PriceCostChangeListMap : EntityTypeConfiguration<Pro_PriceCostChangeList>
    {
        public Pro_PriceCostChangeListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_PriceCostChangeList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.OldCostPrice).HasColumnName("OldCostPrice");
            this.Property(t => t.NewCostPrice).HasColumnName("NewCostPrice");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.NewRetailPrice).HasColumnName("NewRetailPrice");
            this.Property(t => t.OldRetailPrice).HasColumnName("OldRetailPrice");

            // Relationships
            this.HasOptional(t => t.Pro_PriceCostChange)
                .WithMany(t => t.Pro_PriceCostChangeList)
                .HasForeignKey(d => d.ChangeID);

        }
    }
}
