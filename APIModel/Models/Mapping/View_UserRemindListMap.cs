using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_UserRemindListMap : EntityTypeConfiguration<View_UserRemindList>
    {
        public View_UserRemindListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.ProcName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.MenuValue)
                .HasMaxLength(100);

            this.Property(t => t.MenuText)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_UserRemindList");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Count).HasColumnName("Count");
            this.Property(t => t.IsInTime).HasColumnName("IsInTime");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.MenuID).HasColumnName("MenuID");
            this.Property(t => t.ProcName).HasColumnName("ProcName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MenuValue).HasColumnName("MenuValue");
            this.Property(t => t.MenuText).HasColumnName("MenuText");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
