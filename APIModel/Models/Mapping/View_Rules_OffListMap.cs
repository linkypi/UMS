using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Rules_OffListMap : EntityTypeConfiguration<View_Rules_OffList>
    {
        public View_Rules_OffListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Flag, t.ID, t.State });

            // Properties
            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.State)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Rules_OffList");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
        }
    }
}
