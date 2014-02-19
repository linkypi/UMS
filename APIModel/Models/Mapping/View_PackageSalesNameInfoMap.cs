using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_PackageSalesNameInfoMap : EntityTypeConfiguration<View_PackageSalesNameInfo>
    {
        public View_PackageSalesNameInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.SalesName)
                .HasMaxLength(50);

            this.Property(t => t.OldSalesName)
                .HasMaxLength(50);

            this.Property(t => t.OldNote)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("View_PackageSalesNameInfo");
            this.Property(t => t.Parent).HasColumnName("Parent");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SalesName).HasColumnName("SalesName");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OldSalesName).HasColumnName("OldSalesName");
            this.Property(t => t.OldNote).HasColumnName("OldNote");
        }
    }
}
