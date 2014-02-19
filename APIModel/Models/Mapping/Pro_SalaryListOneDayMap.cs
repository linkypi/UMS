using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SalaryListOneDayMap : EntityTypeConfiguration<Pro_SalaryListOneDay>
    {
        public Pro_SalaryListOneDayMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SalaryListOneDay");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.BaseSalary).HasColumnName("BaseSalary");
            this.Property(t => t.SpecalSalary).HasColumnName("SpecalSalary");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
            this.Property(t => t.SalaryDay).HasColumnName("SalaryDay");
        }
    }
}
