using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_Role_MenuInfoMap : EntityTypeConfiguration<Sys_Role_MenuInfo>
    {
        public Sys_Role_MenuInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_Role_MenuInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsChecked).HasColumnName("IsChecked");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_Role_MenuInfo)
                .HasForeignKey(d => d.MenuID);
            this.HasOptional(t => t.Sys_RoleInfo)
                .WithMany(t => t.Sys_Role_MenuInfo)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
