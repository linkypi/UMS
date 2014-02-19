using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_Role_Menu_ProInfoMap : EntityTypeConfiguration<Sys_Role_Menu_ProInfo>
    {
        public Sys_Role_Menu_ProInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Sys_Role_Menu_ProInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            // Relationships
            this.HasOptional(t => t.Pro_ClassInfo)
                .WithMany(t => t.Sys_Role_Menu_ProInfo)
                .HasForeignKey(d => d.ClassID);
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_Role_Menu_ProInfo)
                .HasForeignKey(d => d.MenuID);
            this.HasOptional(t => t.Sys_RoleInfo)
                .WithMany(t => t.Sys_Role_Menu_ProInfo)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
