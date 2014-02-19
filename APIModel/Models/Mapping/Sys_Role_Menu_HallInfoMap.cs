using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_Role_Menu_HallInfoMap : EntityTypeConfiguration<Sys_Role_Menu_HallInfo>
    {
        public Sys_Role_Menu_HallInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Sys_Role_Menu_HallInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Sys_Role_Menu_HallInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_Role_Menu_HallInfo)
                .HasForeignKey(d => d.MenuID);
            this.HasOptional(t => t.Sys_RoleInfo)
                .WithMany(t => t.Sys_Role_Menu_HallInfo)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
