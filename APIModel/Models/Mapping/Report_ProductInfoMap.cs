using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_ProductInfoMap : EntityTypeConfiguration<Report_ProductInfo>
    {
        public Report_ProductInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.商品编码, t.有串码, t.属于服务, t.日期之前可用, t.日期之前加, t.日期之后可用, t.日期之后加, t.兑券临界值, t.小于临界值加, t.大于临界值加, t.需要补差 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.商品编码)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.会员类别)
                .HasMaxLength(50);

            this.Property(t => t.有串码)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.属于服务)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.日期之前可用)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.日期之前加)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.日期之后可用)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.日期之后加)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.兑券临界值)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.小于临界值加)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.大于临界值加)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.需要补差)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_ProductInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.商品编码).HasColumnName("商品编码");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.会员类别).HasColumnName("会员类别");
            this.Property(t => t.有串码).HasColumnName("有串码");
            this.Property(t => t.属于服务).HasColumnName("属于服务");
            this.Property(t => t.兑券临界日期).HasColumnName("兑券临界日期");
            this.Property(t => t.日期之前可用).HasColumnName("日期之前可用");
            this.Property(t => t.日期之前加).HasColumnName("日期之前加");
            this.Property(t => t.日期之后可用).HasColumnName("日期之后可用");
            this.Property(t => t.日期之后加).HasColumnName("日期之后加");
            this.Property(t => t.兑券临界值).HasColumnName("兑券临界值");
            this.Property(t => t.小于临界值加).HasColumnName("小于临界值加");
            this.Property(t => t.大于临界值加).HasColumnName("大于临界值加");
            this.Property(t => t.需要补差).HasColumnName("需要补差");
            this.Property(t => t.备注).HasColumnName("备注");
        }
    }
}
