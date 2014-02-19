using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_SellTypeInfoMap : EntityTypeConfiguration<Rules_SellTypeInfo>
    {
        public Rules_SellTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Rules_SellTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RulesID).HasColumnName("RulesID");
            this.Property(t => t.SellType).HasColumnName("SellType");

            // Relationships
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Rules_SellTypeInfo)
                .HasForeignKey(d => d.SellType);
            this.HasOptional(t => t.Rules_OffList)
                .WithMany(t => t.Rules_SellTypeInfo)
                .HasForeignKey(d => d.RulesID);

        }
    }
}
