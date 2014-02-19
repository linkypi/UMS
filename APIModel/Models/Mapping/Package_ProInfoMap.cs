using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Package_ProInfoMap : EntityTypeConfiguration<Package_ProInfo>
    {
        public Package_ProInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Package_ProInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.ProMainNameID).HasColumnName("ProMainNameID");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Package_GroupInfo)
                .WithMany(t => t.Package_ProInfo)
                .HasForeignKey(d => d.GroupID);

        }
    }
}
