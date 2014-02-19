using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Print_SellBackListInfoMap : EntityTypeConfiguration<Print_SellBackListInfo>
    {
        public Print_SellBackListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.自增主键编号, t.备注 });

            // Properties
            this.Property(t => t.自增主键编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.商品名称)
                .HasMaxLength(100);

            this.Property(t => t.串码_号码_合同号)
                .HasMaxLength(50);

            this.Property(t => t.券号_合约号)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.退货单号)
                .HasMaxLength(50);

            this.Property(t => t.销售员)
                .HasMaxLength(50);

            this.Property(t => t.销售门店)
                .HasMaxLength(50);

            this.Property(t => t.销售公司)
                .HasMaxLength(50);

            this.Property(t => t.会员卡号)
                .HasMaxLength(50);

            this.Property(t => t.客户电姓名)
                .HasMaxLength(50);

            this.Property(t => t.客户电话)
                .HasMaxLength(50);

            this.Property(t => t.优惠券名称)
                .HasMaxLength(50);

            this.Property(t => t.实收总额大写)
                .HasMaxLength(200);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Print_SellBackListInfo");
            this.Property(t => t.自增主键编号).HasColumnName("自增主键编号");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.串码_号码_合同号).HasColumnName("串码/号码/合同号");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.券面值).HasColumnName("券面值");
            this.Property(t => t.券号_合约号).HasColumnName("券号/合约号");
            this.Property(t => t.优惠).HasColumnName("优惠");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.金额小计).HasColumnName("金额小计");
            this.Property(t => t.退货单号).HasColumnName("退货单号");
            this.Property(t => t.销售员).HasColumnName("销售员");
            this.Property(t => t.销售日期).HasColumnName("销售日期");
            this.Property(t => t.销售门店).HasColumnName("销售门店");
            this.Property(t => t.销售公司).HasColumnName("销售公司");
            this.Property(t => t.会员卡号).HasColumnName("会员卡号");
            this.Property(t => t.客户电姓名).HasColumnName("客户电姓名");
            this.Property(t => t.客户电话).HasColumnName("客户电话");
            this.Property(t => t.应收总额).HasColumnName("应收总额");
            this.Property(t => t.优惠券名称).HasColumnName("优惠券名称");
            this.Property(t => t.优惠券金额).HasColumnName("优惠券金额");
            this.Property(t => t.实收总额).HasColumnName("实收总额");
            this.Property(t => t.实收总额大写).HasColumnName("实收总额大写");
            this.Property(t => t.刷卡).HasColumnName("刷卡");
            this.Property(t => t.现金).HasColumnName("现金");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
        }
    }
}
