using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_SalaryCurrentListDeleteListMap : EntityTypeConfiguration<Sys_SalaryCurrentListDeleteList>
    {
        public Sys_SalaryCurrentListDeleteListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_SalaryCurrentListDeleteList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.BaseSalary).HasColumnName("BaseSalary");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.SpecialSalary).HasColumnName("SpecialSalary");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
            this.Property(t => t.SalaryDay).HasColumnName("SalaryDay");
        }
    }
}
