using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_SalaryTempMap : EntityTypeConfiguration<Sys_SalaryTemp>
    {
        public Sys_SalaryTempMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProClassName)
                .HasMaxLength(50);

            this.Property(t => t.ProTypeName)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_SalaryTemp");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.ProClassName).HasColumnName("ProClassName");
            this.Property(t => t.ProTypeName).HasColumnName("ProTypeName");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.SalaryYear).HasColumnName("SalaryYear");
            this.Property(t => t.SalaryMonth).HasColumnName("SalaryMonth");
            this.Property(t => t.C1).HasColumnName("1");
            this.Property(t => t.C2).HasColumnName("2");
            this.Property(t => t.C3).HasColumnName("3");
            this.Property(t => t.C4).HasColumnName("4");
            this.Property(t => t.C5).HasColumnName("5");
            this.Property(t => t.C6).HasColumnName("6");
            this.Property(t => t.C7).HasColumnName("7");
            this.Property(t => t.C8).HasColumnName("8");
            this.Property(t => t.C9).HasColumnName("9");
            this.Property(t => t.C10).HasColumnName("10");
            this.Property(t => t.C11).HasColumnName("11");
            this.Property(t => t.C12).HasColumnName("12");
            this.Property(t => t.C13).HasColumnName("13");
            this.Property(t => t.C14).HasColumnName("14");
            this.Property(t => t.C15).HasColumnName("15");
            this.Property(t => t.C16).HasColumnName("16");
            this.Property(t => t.C17).HasColumnName("17");
            this.Property(t => t.C18).HasColumnName("18");
            this.Property(t => t.C19).HasColumnName("19");
            this.Property(t => t.C20).HasColumnName("20");
            this.Property(t => t.C21).HasColumnName("21");
            this.Property(t => t.C22).HasColumnName("22");
            this.Property(t => t.C23).HasColumnName("23");
            this.Property(t => t.C24).HasColumnName("24");
            this.Property(t => t.C25).HasColumnName("25");
            this.Property(t => t.C26).HasColumnName("26");
            this.Property(t => t.C27).HasColumnName("27");
            this.Property(t => t.C28).HasColumnName("28");
            this.Property(t => t.C29).HasColumnName("29");
            this.Property(t => t.C30).HasColumnName("30");
        }
    }
}
