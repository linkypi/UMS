using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_PriceCost_InorderListMap : EntityTypeConfiguration<Pro_PriceCost_InorderList>
    {
        public Pro_PriceCost_InorderListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_PriceCost_InorderList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CostChangeID).HasColumnName("CostChangeID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.OldCost).HasColumnName("OldCost");
            this.Property(t => t.NewCost).HasColumnName("NewCost");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OldRetailPrice).HasColumnName("OldRetailPrice");
            this.Property(t => t.NewRetailPrice).HasColumnName("NewRetailPrice");

            // Relationships
            this.HasOptional(t => t.Pro_PriceCostChange)
                .WithMany(t => t.Pro_PriceCost_InorderList)
                .HasForeignKey(d => d.CostChangeID);

        }
    }
}
