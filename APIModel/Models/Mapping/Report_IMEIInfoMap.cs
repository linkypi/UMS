using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_IMEIInfoMap : EntityTypeConfiguration<Report_IMEIInfo>
    {
        public Report_IMEIInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.状态, t.库龄_天 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.串码)
                .HasMaxLength(50);

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

            this.Property(t => t.状态)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.库龄_天)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_IMEIInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.状态).HasColumnName("状态");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.库龄_天).HasColumnName("库龄_天");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
