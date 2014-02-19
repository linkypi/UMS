using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_MenuInfoMap : EntityTypeConfiguration<Sys_MenuInfo>
    {
        public Sys_MenuInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.MenuID);

            // Properties
            this.Property(t => t.MenuText)
                .HasMaxLength(50);

            this.Property(t => t.MenuValue)
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.MenuImg)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_MenuInfo");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.MenuText).HasColumnName("MenuText");
            this.Property(t => t.MenuValue).HasColumnName("MenuValue");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Parent).HasColumnName("Parent");
            this.Property(t => t.MenuImg).HasColumnName("MenuImg");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.HasHallRole).HasColumnName("HasHallRole");
            this.Property(t => t.HasProRole).HasColumnName("HasProRole");
            this.Property(t => t.DisplayMobile).HasColumnName("DisplayMobile");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo2)
                .WithMany(t => t.Sys_MenuInfo1)
                .HasForeignKey(d => d.Parent);

        }
    }
}
