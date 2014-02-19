using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_UserOpMap : EntityTypeConfiguration<Sys_UserOp>
    {
        public Sys_UserOpMap()
        {
            // Primary Key
            this.HasKey(t => t.OpID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_UserOp");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
