using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_MethodInfoMap : EntityTypeConfiguration<Sys_MethodInfo>
    {
        public Sys_MethodInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.MethodID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.MethodName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.DllName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Log)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_MethodInfo");
            this.Property(t => t.MethodID).HasColumnName("MethodID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MethodName).HasColumnName("MethodName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.DllName).HasColumnName("DllName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Log).HasColumnName("Log");
            this.Property(t => t.Validity).HasColumnName("Validity");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_MethodInfo)
                .HasForeignKey(d => d.MenuID);

        }
    }
}
