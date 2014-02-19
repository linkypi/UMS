using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_SalaryChangeMap : EntityTypeConfiguration<Sys_SalaryChange>
    {
        public Sys_SalaryChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ChangeID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_SalaryChange");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");

            // Relationships
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Sys_SalaryChange)
                .HasForeignKey(d => d.UserID);

        }
    }
}
