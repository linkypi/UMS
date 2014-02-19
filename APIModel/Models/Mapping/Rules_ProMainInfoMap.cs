using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_ProMainInfoMap : EntityTypeConfiguration<Rules_ProMainInfo>
    {
        public Rules_ProMainInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Rules_ProMainInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RulesID).HasColumnName("RulesID");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");

            // Relationships
            this.HasOptional(t => t.Pro_ProMainInfo)
                .WithMany(t => t.Rules_ProMainInfo)
                .HasForeignKey(d => d.ProMainID);
            this.HasOptional(t => t.Rules_OffList)
                .WithMany(t => t.Rules_ProMainInfo)
                .HasForeignKey(d => d.RulesID);

        }
    }
}
