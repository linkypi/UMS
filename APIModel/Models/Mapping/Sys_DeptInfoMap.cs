using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_DeptInfoMap : EntityTypeConfiguration<Sys_DeptInfo>
    {
        public Sys_DeptInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.DtpID);

            // Properties
            this.Property(t => t.DtpName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Head)
                .HasMaxLength(50);

            this.Property(t => t.HeadTele)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_DeptInfo");
            this.Property(t => t.DtpID).HasColumnName("DtpID");
            this.Property(t => t.Parent).HasColumnName("Parent");
            this.Property(t => t.DtpName).HasColumnName("DtpName");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Head).HasColumnName("Head");
            this.Property(t => t.HeadTele).HasColumnName("HeadTele");

            // Relationships
            this.HasOptional(t => t.Sys_DeptInfo2)
                .WithMany(t => t.Sys_DeptInfo1)
                .HasForeignKey(d => d.Parent);

        }
    }
}
