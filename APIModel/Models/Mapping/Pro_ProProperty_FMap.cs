using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ProProperty_FMap : EntityTypeConfiguration<Pro_ProProperty_F>
    {
        public Pro_ProProperty_FMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ProProperty_F");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ValueID).HasColumnName("ValueID");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_ProProperty_F)
                .HasForeignKey(d => d.ProID);
            this.HasRequired(t => t.Pro_PropertyValue)
                .WithMany(t => t.Pro_ProProperty_F)
                .HasForeignKey(d => d.ValueID);

        }
    }
}
