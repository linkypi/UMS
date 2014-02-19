using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_SalaryCurrentListMap : EntityTypeConfiguration<Sys_SalaryCurrentList>
    {
        public Sys_SalaryCurrentListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_SalaryCurrentList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.BaseSalary).HasColumnName("BaseSalary");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.SpecialSalary).HasColumnName("SpecialSalary");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
            this.Property(t => t.SalaryDay).HasColumnName("SalaryDay");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Sys_SalaryCurrentList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Sys_SalaryCurrentList)
                .HasForeignKey(d => d.SellType);
            this.HasOptional(t => t.Sys_UserOp)
                .WithMany(t => t.Sys_SalaryCurrentList)
                .HasForeignKey(d => d.OpID);

        }
    }
}
