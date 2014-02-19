using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_RemindListMap : EntityTypeConfiguration<View_RemindList>
    {
        public View_RemindListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IsInTime, t.ID, t.OldIsInTime, t.Flag, t.OldFlag });

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.ProcName)
                .HasMaxLength(50);

            this.Property(t => t.IsInTime)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.OldName)
                .HasMaxLength(50);

            this.Property(t => t.OldNote)
                .HasMaxLength(500);

            this.Property(t => t.OldProcName)
                .HasMaxLength(50);

            this.Property(t => t.OldIsInTime)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Flag)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.OldFlag)
                .IsRequired()
                .HasMaxLength(6);

            // Table & Column Mappings
            this.ToTable("View_RemindList");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProcName).HasColumnName("ProcName");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.IsInTime).HasColumnName("IsInTime");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OldName).HasColumnName("OldName");
            this.Property(t => t.OldNote).HasColumnName("OldNote");
            this.Property(t => t.OldProcName).HasColumnName("OldProcName");
            this.Property(t => t.OldMenuID).HasColumnName("OldMenuID");
            this.Property(t => t.OldIsInTime).HasColumnName("OldIsInTime");
            this.Property(t => t.OldOrder).HasColumnName("OldOrder");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.OldFlag).HasColumnName("OldFlag");
        }
    }
}
