using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Chart_MapReportMap : EntityTypeConfiguration<Chart_MapReport>
    {
        public Chart_MapReportMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.HallID, t.ProClass, t.DATE, t.AreaID, t.Profit, t.ClassTypeName, t.AsPrice });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.ProClass)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.AreaName)
                .HasMaxLength(50);

            this.Property(t => t.AreaID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Profit)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BigAreaName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Chart_MapReport");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.ProClass).HasColumnName("ProClass");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.DATE).HasColumnName("DATE");
            this.Property(t => t.Sells).HasColumnName("Sells");
            this.Property(t => t.SellPrice).HasColumnName("SellPrice");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.Profit).HasColumnName("Profit");
            this.Property(t => t.ClassTypeName).HasColumnName("ClassTypeName");
            this.Property(t => t.AsPrice).HasColumnName("AsPrice");
            this.Property(t => t.BigAreaID).HasColumnName("BigAreaID");
            this.Property(t => t.BigAreaName).HasColumnName("BigAreaName");
        }
    }
}
