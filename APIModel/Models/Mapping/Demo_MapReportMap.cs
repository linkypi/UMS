using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_MapReportMap : EntityTypeConfiguration<Demo_MapReport>
    {
        public Demo_MapReportMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AreaName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_MapReport");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Sells).HasColumnName("Sells");
            this.Property(t => t.SellPrice).HasColumnName("SellPrice");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProClass).HasColumnName("ProClass");
            this.Property(t => t.Profit).HasColumnName("Profit");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
        }
    }
}
