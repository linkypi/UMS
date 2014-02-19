using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_HallInfoMap : EntityTypeConfiguration<View_HallInfo>
    {
        public View_HallInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HallID, t.IsCanIn, t.IsCanback });

            // Properties
            this.Property(t => t.AreaName)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DisPlayName)
                .HasMaxLength(50);

            this.Property(t => t.ShortName)
                .HasMaxLength(50);

            this.Property(t => t.PrintName)
                .HasMaxLength(50);

            this.Property(t => t.IsCanIn)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.IsCanback)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.LevelName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_HallInfo");
            this.Property(t => t.AreaName).HasColumnName("AreaName");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            this.Property(t => t.LevelID).HasColumnName("LevelID");
            this.Property(t => t.CanIn).HasColumnName("CanIn");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CanBack).HasColumnName("CanBack");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Longitude).HasColumnName("Longitude");
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.DisPlayName).HasColumnName("DisPlayName");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.SellNum).HasColumnName("SellNum");
            this.Property(t => t.PrintName).HasColumnName("PrintName");
            this.Property(t => t.IsCanIn).HasColumnName("IsCanIn");
            this.Property(t => t.IsCanback).HasColumnName("IsCanback");
            this.Property(t => t.LevelName).HasColumnName("LevelName");
        }
    }
}
