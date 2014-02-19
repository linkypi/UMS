using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_Pro_RulesTypeInfoMap : EntityTypeConfiguration<Rules_Pro_RulesTypeInfo>
    {
        public Rules_Pro_RulesTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Rules_Pro_RulesTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RulesProID).HasColumnName("RulesProID");
            this.Property(t => t.RulesTypeID).HasColumnName("RulesTypeID");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.OrderBy).HasColumnName("OrderBy");

            // Relationships
            this.HasOptional(t => t.Rules_ProMainInfo)
                .WithMany(t => t.Rules_Pro_RulesTypeInfo)
                .HasForeignKey(d => d.RulesProID);
            this.HasOptional(t => t.Rules_TypeInfo)
                .WithMany(t => t.Rules_Pro_RulesTypeInfo)
                .HasForeignKey(d => d.RulesTypeID);

        }
    }
}
