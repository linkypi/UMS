using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_IMEITracksInfoMap : EntityTypeConfiguration<Report_IMEITracksInfo>
    {
        public Report_IMEITracksInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.系统平台 });

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

            this.Property(t => t.商品属性)
                .HasMaxLength(100);

            this.Property(t => t.跟踪)
                .HasMaxLength(129);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.系统平台)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_IMEITracksInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.跟踪).HasColumnName("跟踪");
            this.Property(t => t.日期).HasColumnName("日期");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.系统平台).HasColumnName("系统平台");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
