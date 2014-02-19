using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_RemindListMap : EntityTypeConfiguration<Sys_RemindList>
    {
        public Sys_RemindListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.ProcName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_RemindList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProcName).HasColumnName("ProcName");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.IsInTime).HasColumnName("IsInTime");
            this.Property(t => t.Count).HasColumnName("Count");

            // Relationships
            this.HasOptional(t => t.Sys_MenuInfo)
                .WithMany(t => t.Sys_RemindList)
                .HasForeignKey(d => d.MenuID);

        }
    }
}
