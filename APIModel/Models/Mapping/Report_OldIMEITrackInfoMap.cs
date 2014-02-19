using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_OldIMEITrackInfoMap : EntityTypeConfiguration<Report_OldIMEITrackInfo>
    {
        public Report_OldIMEITrackInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProFormat, t.plat });

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(100);

            this.Property(t => t.ClassName)
                .HasMaxLength(100);

            this.Property(t => t.TypeName)
                .HasMaxLength(100);

            this.Property(t => t.proname)
                .HasMaxLength(100);

            this.Property(t => t.ProFormat)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Note)
                .HasMaxLength(56);

            this.Property(t => t.InUserName)
                .HasMaxLength(100);

            this.Property(t => t.plat)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.hallid)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_OldIMEITrackInfo");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.proname).HasColumnName("proname");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InDate).HasColumnName("InDate");
            this.Property(t => t.InUserName).HasColumnName("InUserName");
            this.Property(t => t.plat).HasColumnName("plat");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.hallid).HasColumnName("hallid");
        }
    }
}
