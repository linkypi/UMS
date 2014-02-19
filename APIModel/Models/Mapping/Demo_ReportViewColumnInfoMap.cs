using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_ReportViewColumnInfoMap : EntityTypeConfiguration<Demo_ReportViewColumnInfo>
    {
        public Demo_ReportViewColumnInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ColDisPlayName)
                .HasMaxLength(50);

            this.Property(t => t.ColName)
                .HasMaxLength(50);

            this.Property(t => t.FormatStr)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Demo_ReportViewColumnInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ReportID).HasColumnName("ReportID");
            this.Property(t => t.ColDisPlayName).HasColumnName("ColDisPlayName");
            this.Property(t => t.ColName).HasColumnName("ColName");
            this.Property(t => t.OrderBy).HasColumnName("OrderBy");
            this.Property(t => t.FormatStr).HasColumnName("FormatStr");

            // Relationships
            this.HasOptional(t => t.Demo_ReportViewInfo)
                .WithMany(t => t.Demo_ReportViewColumnInfo)
                .HasForeignKey(d => d.ReportID);

        }
    }
}
