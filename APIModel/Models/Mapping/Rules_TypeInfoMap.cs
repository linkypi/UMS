using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_TypeInfoMap : EntityTypeConfiguration<Rules_TypeInfo>
    {
        public Rules_TypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RulesName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Rules_TypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RulesName).HasColumnName("RulesName");
            this.Property(t => t.ShowToCus).HasColumnName("ShowToCus");
            this.Property(t => t.CanGetBack).HasColumnName("CanGetBack");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
