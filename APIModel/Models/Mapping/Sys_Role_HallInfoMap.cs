using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_Role_HallInfoMap : EntityTypeConfiguration<Sys_Role_HallInfo>
    {
        public Sys_Role_HallInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Sys_Role_HallInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Role_Menu_ID).HasColumnName("Role_Menu_ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.RoleID).HasColumnName("RoleID");

            // Relationships
            this.HasRequired(t => t.Sys_Role_MenuInfo)
                .WithOptional(t => t.Sys_Role_HallInfo);

        }
    }
}
