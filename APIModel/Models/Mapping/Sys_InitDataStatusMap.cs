using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_InitDataStatusMap : EntityTypeConfiguration<Sys_InitDataStatus>
    {
        public Sys_InitDataStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DataName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.DLLName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_InitDataStatus");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DataName).HasColumnName("DataName");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.DLLName).HasColumnName("DLLName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
        }
    }
}
