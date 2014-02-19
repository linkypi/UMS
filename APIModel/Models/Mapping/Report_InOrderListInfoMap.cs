using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_InOrderListInfoMap : EntityTypeConfiguration<Report_InOrderListInfo>
    {
        public Report_InOrderListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.批次号);

            // Properties
            this.Property(t => t.批次号)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.入库单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.入库仓库)
                .HasMaxLength(50);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.入库仓库编码)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_InOrderListInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.成本价).HasColumnName("成本价");
            this.Property(t => t.入库单号).HasColumnName("入库单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.入库仓库).HasColumnName("入库仓库");
            this.Property(t => t.入库日期).HasColumnName("入库日期");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.入库仓库编码).HasColumnName("入库仓库编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.备注).HasColumnName("备注");
        }
    }
}
