using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_PriceInfoMap : EntityTypeConfiguration<Report_PriceInfo>
    {
        public Report_PriceInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.价格, t.结算价, t.最低价格, t.最高价格, t.可以兑券, t.需要审批单 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.销售类别)
                .HasMaxLength(50);

            this.Property(t => t.价格)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.结算价)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.最低价格)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.最高价格)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.可以兑券)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.需要审批单)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Report_PriceInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.销售类别).HasColumnName("销售类别");
            this.Property(t => t.价格).HasColumnName("价格");
            this.Property(t => t.结算价).HasColumnName("结算价");
            this.Property(t => t.最低价格).HasColumnName("最低价格");
            this.Property(t => t.最高价格).HasColumnName("最高价格");
            this.Property(t => t.可以兑券).HasColumnName("可以兑券");
            this.Property(t => t.需要审批单).HasColumnName("需要审批单");
        }
    }
}
