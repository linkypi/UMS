using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_SellChartMap : EntityTypeConfiguration<Report_SellChart>
    {
        public Report_SellChartMap()
        {
            // Primary Key
            this.HasKey(t => new { t.day, t.M1, t.M2 });

            // Properties
            this.Property(t => t.day)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.M1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.M2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Report_SellChart");
            this.Property(t => t.day).HasColumnName("day");
            this.Property(t => t.M1).HasColumnName("M1");
            this.Property(t => t.M2).HasColumnName("M2");
        }
    }
}
