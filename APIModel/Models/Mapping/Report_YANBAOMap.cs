using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_YANBAOMap : EntityTypeConfiguration<Report_YANBAO>
    {
        public Report_YANBAOMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.备注, t.状态 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.大区)
                .HasMaxLength(50);

            this.Property(t => t.区域)
                .HasMaxLength(50);

            this.Property(t => t.店名)
                .HasMaxLength(50);

            this.Property(t => t.网店属性)
                .HasMaxLength(50);

            this.Property(t => t.销售员)
                .HasMaxLength(50);

            this.Property(t => t.仓管员)
                .HasMaxLength(50);

            this.Property(t => t.客户姓名)
                .HasMaxLength(50);

            this.Property(t => t.联系方式)
                .HasMaxLength(50);

            this.Property(t => t.合同编号)
                .HasMaxLength(50);

            this.Property(t => t.手机型号)
                .HasMaxLength(50);

            this.Property(t => t.手机串号)
                .HasMaxLength(50);

            this.Property(t => t.手机购买方式)
                .HasMaxLength(50);

            this.Property(t => t.发票号码)
                .HasMaxLength(50);

            this.Property(t => t.电池编码)
                .HasMaxLength(50);

            this.Property(t => t.充电器编码)
                .HasMaxLength(50);

            this.Property(t => t.耳机编码)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .IsRequired()
                .HasMaxLength(550);

            this.Property(t => t.状态)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_YANBAO");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.录入时间).HasColumnName("录入时间");
            this.Property(t => t.大区).HasColumnName("大区");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.店名).HasColumnName("店名");
            this.Property(t => t.网店属性).HasColumnName("网店属性");
            this.Property(t => t.销售员).HasColumnName("销售员");
            this.Property(t => t.仓管员).HasColumnName("仓管员");
            this.Property(t => t.客户姓名).HasColumnName("客户姓名");
            this.Property(t => t.联系方式).HasColumnName("联系方式");
            this.Property(t => t.延保购买日期).HasColumnName("延保购买日期");
            this.Property(t => t.合同编号).HasColumnName("合同编号");
            this.Property(t => t.销售数量).HasColumnName("销售数量");
            this.Property(t => t.延保价格).HasColumnName("延保价格");
            this.Property(t => t.手机型号).HasColumnName("手机型号");
            this.Property(t => t.手机价格).HasColumnName("手机价格");
            this.Property(t => t.手机串号).HasColumnName("手机串号");
            this.Property(t => t.手机购买方式).HasColumnName("手机购买方式");
            this.Property(t => t.发票号码).HasColumnName("发票号码");
            this.Property(t => t.电池编码).HasColumnName("电池编码");
            this.Property(t => t.充电器编码).HasColumnName("充电器编码");
            this.Property(t => t.耳机编码).HasColumnName("耳机编码");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.状态).HasColumnName("状态");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
