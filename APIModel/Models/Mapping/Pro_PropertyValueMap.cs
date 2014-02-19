using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_PropertyValueMap : EntityTypeConfiguration<Pro_PropertyValue>
    {
        public Pro_PropertyValueMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Value)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_PropertyValue");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Pro_PropertyID).HasColumnName("Pro_PropertyID");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_Property)
                .WithMany(t => t.Pro_PropertyValue)
                .HasForeignKey(d => d.Pro_PropertyID);

        }
    }
}
