using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VersionMap : EntityTypeConfiguration<VIP_Version>
    {
        public VIP_VersionMap()
        {
            // Primary Key
            this.HasKey(t => t.versionId);

            // Properties
            this.Property(t => t.versionDesc)
                .HasMaxLength(300);

            this.Property(t => t.versionName)
                .HasMaxLength(50);

            this.Property(t => t.versionNo)
                .HasMaxLength(50);

            this.Property(t => t.versionSrc)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("VIP_Version");
            this.Property(t => t.versionId).HasColumnName("versionId");
            this.Property(t => t.versionDesc).HasColumnName("versionDesc");
            this.Property(t => t.versionName).HasColumnName("versionName");
            this.Property(t => t.versionNo).HasColumnName("versionNo");
            this.Property(t => t.versionSrc).HasColumnName("versionSrc");
            this.Property(t => t.versionAddtime).HasColumnName("versionAddtime");
        }
    }
}
