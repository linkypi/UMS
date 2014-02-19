using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_ReportViewInfoMap : EntityTypeConfiguration<Demo_ReportViewInfo>
    {
        public Demo_ReportViewInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ReportViewName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_ReportViewInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ReportViewName).HasColumnName("ReportViewName");
            this.Property(t => t.MenuID).HasColumnName("MenuID");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Demo_ReportViewInfo)
                .HasForeignKey(d => d.MenuID);

        }
    }
}
