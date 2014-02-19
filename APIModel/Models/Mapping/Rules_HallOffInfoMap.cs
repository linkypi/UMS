using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_HallOffInfoMap : EntityTypeConfiguration<Rules_HallOffInfo>
    {
        public Rules_HallOffInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Rules_HallOffInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.RulesID).HasColumnName("RulesID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Rules_HallOffInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Rules_OffList)
                .WithMany(t => t.Rules_HallOffInfo)
                .HasForeignKey(d => d.RulesID);

        }
    }
}
