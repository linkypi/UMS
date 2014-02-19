using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_YanbaoPriceStepInfoMap : EntityTypeConfiguration<Pro_YanbaoPriceStepInfo>
    {
        public Pro_YanbaoPriceStepInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Pro_YanbaoPriceStepInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.StepPrice).HasColumnName("StepPrice");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");

            // Relationships
            this.HasOptional(t => t.Pro_PriceChange)
                .WithMany(t => t.Pro_YanbaoPriceStepInfo)
                .HasForeignKey(d => d.ChangeID);

        }
    }
}