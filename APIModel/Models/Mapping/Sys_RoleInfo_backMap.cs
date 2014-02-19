using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_RoleInfo_backMap : EntityTypeConfiguration<Sys_RoleInfo_back>
    {
        public Sys_RoleInfo_backMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RoleName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Updater)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_RoleInfo_back");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.Menu_ID_List).HasColumnName("Menu_ID_List");
            this.Property(t => t.Method_ID_List).HasColumnName("Method_ID_List");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.MenuXML).HasColumnName("MenuXML");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.Updater).HasColumnName("Updater");
        }
    }
}
