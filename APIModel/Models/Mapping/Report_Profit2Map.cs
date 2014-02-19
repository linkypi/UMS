using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_Profit2Map : EntityTypeConfiguration<Report_Profit2>
    {
        public Report_Profit2Map()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.计算成本价 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.销售单号)
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.销售方式)
                .HasMaxLength(50);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.商品大类)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            this.Property(t => t.商品编码)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .HasMaxLength(50);

            this.Property(t => t.计算成本价)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.片区)
                .HasMaxLength(50);

            this.Property(t => t.大区)
                .HasMaxLength(50);

            this.Property(t => t.销售时间)
                .HasMaxLength(12);

            this.Property(t => t.串码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_Profit2");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.销售单号).HasColumnName("销售单号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.销售方式).HasColumnName("销售方式");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.销售日期).HasColumnName("销售日期");
            this.Property(t => t.兑换值).HasColumnName("兑换值");
            this.Property(t => t.商品大类).HasColumnName("商品大类");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.片区编码).HasColumnName("片区编码");
            this.Property(t => t.大区编码).HasColumnName("大区编码");
            this.Property(t => t.商品编码).HasColumnName("商品编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.商品大类编码).HasColumnName("商品大类编码");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.结算价).HasColumnName("结算价");
            this.Property(t => t.入库成本价).HasColumnName("入库成本价");
            this.Property(t => t.销售时成本价).HasColumnName("销售时成本价");
            this.Property(t => t.计算成本价).HasColumnName("计算成本价");
            this.Property(t => t.实收单价).HasColumnName("实收单价");
            this.Property(t => t.退货审批利润).HasColumnName("退货审批利润");
            this.Property(t => t.利润).HasColumnName("利润");
            this.Property(t => t.片区).HasColumnName("片区");
            this.Property(t => t.大区).HasColumnName("大区");
            this.Property(t => t.销售时间).HasColumnName("销售时间");
            this.Property(t => t.串码).HasColumnName("串码");
        }
    }
}
