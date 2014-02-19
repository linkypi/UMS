using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SalaryListMap : EntityTypeConfiguration<View_SalaryList>
    {
        public View_SalaryListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SalaryList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.BaseSalary).HasColumnName("BaseSalary");
            this.Property(t => t.UpdateFlag).HasColumnName("UpdateFlag");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.SpecialSalary).HasColumnName("SpecialSalary");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryDay).HasColumnName("SalaryDay");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
        }
    }
}
