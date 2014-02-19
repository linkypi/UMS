using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_EveryHallStoreInfoMap : EntityTypeConfiguration<Report_EveryHallStoreInfo>
    {
        public Report_EveryHallStoreInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.ProID, t.门店编码 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.制式)
                .HasMaxLength(50);

            this.Property(t => t.门店等级)
                .HasMaxLength(50);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_EveryHallStoreInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.制式).HasColumnName("制式");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.门店等级).HasColumnName("门店等级");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.等级排序).HasColumnName("等级排序");
            this.Property(t => t.门店排序).HasColumnName("门店排序");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.在途).HasColumnName("在途");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.类别排序).HasColumnName("类别排序");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.属性排序).HasColumnName("属性排序");
        }
    }
}
