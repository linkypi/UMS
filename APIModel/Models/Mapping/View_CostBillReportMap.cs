using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_CostBillReportMap : EntityTypeConfiguration<View_CostBillReport>
    {
        public View_CostBillReportMap()
        {
            // Primary Key
            this.HasKey(t => t.InListID);

            // Properties
            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.StartDate)
                .HasMaxLength(30);

            this.Property(t => t.EndDate)
                .HasMaxLength(30);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_CostBillReport");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.OldCostPrice).HasColumnName("OldCostPrice");
            this.Property(t => t.NewCostPrice).HasColumnName("NewCostPrice");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.InDate).HasColumnName("InDate");
        }
    }
}
