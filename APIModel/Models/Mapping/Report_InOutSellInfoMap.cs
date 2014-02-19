using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_InOutSellInfoMap : EntityTypeConfiguration<Report_InOutSellInfo>
    {
        public Report_InOutSellInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.序号);

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.区域)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_InOutSellInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.期初库存).HasColumnName("期初库存");
            this.Property(t => t.本期初始入库).HasColumnName("本期初始入库");
            this.Property(t => t.本期退库).HasColumnName("本期退库");
            this.Property(t => t.本期类别转入).HasColumnName("本期类别转入");
            this.Property(t => t.本期类别转出).HasColumnName("本期类别转出");
            this.Property(t => t.本期调入).HasColumnName("本期调入");
            this.Property(t => t.本期调出).HasColumnName("本期调出");
            this.Property(t => t.本期销售审批_申请_).HasColumnName("本期销售审批_申请_");
            this.Property(t => t.本期销售审批_已审批_).HasColumnName("本期销售审批_已审批_");
            this.Property(t => t.本期销售).HasColumnName("本期销售");
            this.Property(t => t.本期退货).HasColumnName("本期退货");
            this.Property(t => t.本期送修).HasColumnName("本期送修");
            this.Property(t => t.本期返库).HasColumnName("本期返库");
            this.Property(t => t.本期借贷).HasColumnName("本期借贷");
            this.Property(t => t.本期归还).HasColumnName("本期归还");
            this.Property(t => t.期末库存).HasColumnName("期末库存");
            this.Property(t => t.送修累计).HasColumnName("送修累计");
            this.Property(t => t.借贷累计).HasColumnName("借贷累计");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
