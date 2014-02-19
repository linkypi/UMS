using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_SellReportMap : EntityTypeConfiguration<Report_SellReport>
    {
        public Report_SellReportMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.组合优惠名称 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.销售单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
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

            this.Property(t => t.代金券编码)
                .HasMaxLength(50);

            this.Property(t => t.串码)
                .HasMaxLength(50);

            this.Property(t => t.单品优惠名称)
                .HasMaxLength(50);

            this.Property(t => t.组合优惠名称)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.是否免费服务)
                .HasMaxLength(2);

            this.Property(t => t.销售员)
                .HasMaxLength(50);

            this.Property(t => t.会员卡号)
                .HasMaxLength(50);

            this.Property(t => t.客户姓名)
                .HasMaxLength(50);

            this.Property(t => t.客户电话)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.销售时间)
                .HasMaxLength(12);

            this.Property(t => t.片区)
                .HasMaxLength(50);

            this.Property(t => t.商品大类)
                .HasMaxLength(50);

            this.Property(t => t.单头备注)
                .HasMaxLength(50);

            this.Property(t => t.大区)
                .HasMaxLength(50);

            this.Property(t => t.终端制式)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            this.Property(t => t.商品编码)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .HasMaxLength(50);

            this.Property(t => t.ProMainID)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Report_SellReport");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.销售单号).HasColumnName("销售单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.销售方式).HasColumnName("销售方式");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.实际单价).HasColumnName("实际单价");
            this.Property(t => t.实收金额).HasColumnName("实收金额");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.销售日期).HasColumnName("销售日期");
            this.Property(t => t.代金券编码).HasColumnName("代金券编码");
            this.Property(t => t.代金券面值).HasColumnName("代金券面值");
            this.Property(t => t.兑换值).HasColumnName("兑换值");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.单品优惠名称).HasColumnName("单品优惠名称");
            this.Property(t => t.单品优惠金额).HasColumnName("单品优惠金额");
            this.Property(t => t.组合优惠名称).HasColumnName("组合优惠名称");
            this.Property(t => t.组合优惠金额).HasColumnName("组合优惠金额");
            this.Property(t => t.批发优惠金额).HasColumnName("批发优惠金额");
            this.Property(t => t.多收单价).HasColumnName("多收单价");
            this.Property(t => t.暗补).HasColumnName("暗补");
            this.Property(t => t.列收).HasColumnName("列收");
            this.Property(t => t.终端代销费).HasColumnName("终端代销费");
            this.Property(t => t.是否免费服务).HasColumnName("是否免费服务");
            this.Property(t => t.销售员).HasColumnName("销售员");
            this.Property(t => t.会员卡号).HasColumnName("会员卡号");
            this.Property(t => t.客户姓名).HasColumnName("客户姓名");
            this.Property(t => t.客户电话).HasColumnName("客户电话");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.门店优惠).HasColumnName("门店优惠");
            this.Property(t => t.销售时间).HasColumnName("销售时间");
            this.Property(t => t.片区).HasColumnName("片区");
            this.Property(t => t.商品大类).HasColumnName("商品大类");
            this.Property(t => t.单头备注).HasColumnName("单头备注");
            this.Property(t => t.大区).HasColumnName("大区");
            this.Property(t => t.终端制式).HasColumnName("终端制式");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.片区编码).HasColumnName("片区编码");
            this.Property(t => t.大区编码).HasColumnName("大区编码");
            this.Property(t => t.商品编码).HasColumnName("商品编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.商品大类编码).HasColumnName("商品大类编码");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.销售时成本价).HasColumnName("销售时成本价");
            this.Property(t => t.入库成本价).HasColumnName("入库成本价");
            this.Property(t => t.规则毛利可拿回).HasColumnName("规则毛利可拿回");
            this.Property(t => t.规则毛利不可拿回).HasColumnName("规则毛利不可拿回");
            this.Property(t => t.结算价).HasColumnName("结算价");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.price).HasColumnName("price");
        }
    }
}
