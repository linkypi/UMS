using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_VipBuyTimeInfoMap : EntityTypeConfiguration<Report_VipBuyTimeInfo>
    {
        public Report_VipBuyTimeInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.会员贴膜次数, t.状态 });

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

            this.Property(t => t.卡号)
                .HasMaxLength(50);

            this.Property(t => t.卡类)
                .HasMaxLength(50);

            this.Property(t => t.客户名称)
                .HasMaxLength(50);

            this.Property(t => t.性别)
                .HasMaxLength(10);

            this.Property(t => t.联系号码)
                .HasMaxLength(50);

            this.Property(t => t.备用号码)
                .HasMaxLength(50);

            this.Property(t => t.身份证)
                .HasMaxLength(50);

            this.Property(t => t.详细地址)
                .HasMaxLength(150);

            this.Property(t => t.QQ号_微信_微博)
                .HasMaxLength(50);

            this.Property(t => t.会员贴膜次数)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.状态)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_VipBuyTimeInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.注册时间).HasColumnName("注册时间");
            this.Property(t => t.大区).HasColumnName("大区");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.店名).HasColumnName("店名");
            this.Property(t => t.网店属性).HasColumnName("网店属性");
            this.Property(t => t.销售员).HasColumnName("销售员");
            this.Property(t => t.仓管员).HasColumnName("仓管员");
            this.Property(t => t.卡号).HasColumnName("卡号");
            this.Property(t => t.卡类).HasColumnName("卡类");
            this.Property(t => t.销售金额).HasColumnName("销售金额");
            this.Property(t => t.客户名称).HasColumnName("客户名称");
            this.Property(t => t.性别).HasColumnName("性别");
            this.Property(t => t.生日日期).HasColumnName("生日日期");
            this.Property(t => t.联系号码).HasColumnName("联系号码");
            this.Property(t => t.备用号码).HasColumnName("备用号码");
            this.Property(t => t.身份证).HasColumnName("身份证");
            this.Property(t => t.详细地址).HasColumnName("详细地址");
            this.Property(t => t.QQ号_微信_微博).HasColumnName("QQ号_微信_微博");
            this.Property(t => t.累计积分).HasColumnName("累计积分");
            this.Property(t => t.消费次数).HasColumnName("消费次数");
            this.Property(t => t.累计消费总额).HasColumnName("累计消费总额");
            this.Property(t => t.平均消费水平).HasColumnName("平均消费水平");
            this.Property(t => t.会员贴膜次数).HasColumnName("会员贴膜次数");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.状态).HasColumnName("状态");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
