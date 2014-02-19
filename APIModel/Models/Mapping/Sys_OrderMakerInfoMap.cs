using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_OrderMakerInfoMap : EntityTypeConfiguration<Sys_OrderMakerInfo>
    {
        public Sys_OrderMakerInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderType);

            // Properties
            this.Property(t => t.Header)
                .HasMaxLength(50);

            this.Property(t => t.OrderDate)
                .HasMaxLength(50);

            this.Property(t => t.OrderType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Introduction)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_OrderMakerInfo");
            this.Property(t => t.Header).HasColumnName("Header");
            this.Property(t => t.OrderDate).HasColumnName("OrderDate");
            this.Property(t => t.OrderNO).HasColumnName("OrderNO");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
        }
    }
}
