using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_Role_MenuInfo_bakMap : EntityTypeConfiguration<Sys_Role_MenuInfo_bak>
    {
        public Sys_Role_MenuInfo_bakMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_Role_MenuInfo_bak");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.RoleBakID).HasColumnName("RoleBakID");

            // Relationships
            this.HasRequired(t => t.Sys_RoleInfo_back)
                .WithMany(t => t.Sys_Role_MenuInfo_bak)
                .HasForeignKey(d => d.RoleBakID);

        }
    }
}
