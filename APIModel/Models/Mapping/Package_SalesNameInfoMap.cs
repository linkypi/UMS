using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Package_SalesNameInfoMap : EntityTypeConfiguration<Package_SalesNameInfo>
    {
        public Package_SalesNameInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SalesName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Package_SalesNameInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SalesName).HasColumnName("SalesName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Parent).HasColumnName("Parent");
        }
    }
}
