using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_SalaryListMap : EntityTypeConfiguration<Sys_SalaryList>
    {
        public Sys_SalaryListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_SalaryList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.BaseSalary).HasColumnName("BaseSalary");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.SpecialSalary).HasColumnName("SpecialSalary");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
            this.Property(t => t.SalaryDay).HasColumnName("SalaryDay");
            this.Property(t => t.SysDate).HasColumnName("SysDate");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Sys_SalaryList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Sys_SalaryList)
                .HasForeignKey(d => d.SellType);
            this.HasOptional(t => t.Sys_SalaryChange)
                .WithMany(t => t.Sys_SalaryList)
                .HasForeignKey(d => d.ChangeID);
            this.HasOptional(t => t.Sys_UserOp)
                .WithMany(t => t.Sys_SalaryList)
                .HasForeignKey(d => d.OpID);

        }
    }
}
