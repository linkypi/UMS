using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_CostPriceListInfoMap : EntityTypeConfiguration<Report_CostPriceListInfo>
    {
        public Report_CostPriceListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.批次号, t.成本价 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.成本价)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Report_CostPriceListInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.入库日期).HasColumnName("入库日期");
            this.Property(t => t.成本价).HasColumnName("成本价");
        }
    }
}
