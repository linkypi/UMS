using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_RoleMethodMap : EntityTypeConfiguration<Sys_RoleMethod>
    {
        public Sys_RoleMethodMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_RoleMethod");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.MethodID).HasColumnName("MethodID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.DateLimit).HasColumnName("DateLimit");

            // Relationships
            this.HasOptional(t => t.Pro_ClassInfo)
                .WithMany(t => t.Sys_RoleMethod)
                .HasForeignKey(d => d.ClassID);
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Sys_RoleMethod)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_RoleMethod)
                .HasForeignKey(d => d.MenuID);
            this.HasOptional(t => t.Sys_MethodInfo)
                .WithMany(t => t.Sys_RoleMethod)
                .HasForeignKey(d => d.MethodID);

        }
    }
}
