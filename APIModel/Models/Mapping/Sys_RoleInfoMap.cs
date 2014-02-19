using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_RoleInfoMap : EntityTypeConfiguration<Sys_RoleInfo>
    {
        public Sys_RoleInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleID);

            // Properties
            this.Property(t => t.RoleName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Updater)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_RoleInfo");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.Menu_ID_List).HasColumnName("Menu_ID_List");
            this.Property(t => t.Method_ID_List).HasColumnName("Method_ID_List");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.MenuXML).HasColumnName("MenuXML");
            this.Property(t => t.UpDateTime).HasColumnName("UpDateTime");
            this.Property(t => t.Updater).HasColumnName("Updater");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.MobileMenuJson).HasColumnName("MobileMenuJson");
        }
    }
}
