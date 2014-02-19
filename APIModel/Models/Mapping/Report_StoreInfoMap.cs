using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_StoreInfoMap : EntityTypeConfiguration<Report_StoreInfo>
    {
        public Report_StoreInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.在库数量, t.送修中数量, t.借贷中数量 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.在库数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.送修中数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.借贷中数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.区域)
                .HasMaxLength(50);

            this.Property(t => t.商品编码)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_StoreInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.零售价格).HasColumnName("零售价格");
            this.Property(t => t.在库数量).HasColumnName("在库数量");
            this.Property(t => t.门店优惠审批中).HasColumnName("门店优惠审批中");
            this.Property(t => t.调拨中数量).HasColumnName("调拨中数量");
            this.Property(t => t.送修中数量).HasColumnName("送修中数量");
            this.Property(t => t.借贷中数量).HasColumnName("借贷中数量");
            this.Property(t => t.小计).HasColumnName("小计");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.商品编码).HasColumnName("商品编码");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
