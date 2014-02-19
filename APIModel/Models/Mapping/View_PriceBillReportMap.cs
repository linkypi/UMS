using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_PriceBillReportMap : EntityTypeConfiguration<View_PriceBillReport>
    {
        public View_PriceBillReportMap()
        {
            // Primary Key
            this.HasKey(t => t.IsTicketUseful);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.IsTicketUseful)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_PriceBillReport");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.IsTicketUseful).HasColumnName("IsTicketUseful");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
        }
    }
}
