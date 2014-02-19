using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_UserDefaultOpenPageMap : EntityTypeConfiguration<Sys_UserDefaultOpenPage>
    {
        public Sys_UserDefaultOpenPageMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_UserDefaultOpenPage");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.MenuID).HasColumnName("MenuID");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_UserDefaultOpenPage)
                .HasForeignKey(d => d.MenuID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Sys_UserDefaultOpenPage)
                .HasForeignKey(d => d.UserID);

        }
    }
}
