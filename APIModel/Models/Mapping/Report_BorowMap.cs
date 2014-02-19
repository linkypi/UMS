using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_BorowMap : EntityTypeConfiguration<Report_Borow>
    {
        public Report_BorowMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.审批单)
                .HasMaxLength(50);

            this.Property(t => t.借贷人)
                .HasMaxLength(50);

            this.Property(t => t.借贷日期)
                .HasMaxLength(100);

            this.Property(t => t.借贷部门)
                .HasMaxLength(50);

            this.Property(t => t.借贷方式)
                .HasMaxLength(50);

            this.Property(t => t.商品类别)
                .HasMaxLength(50);

            this.Property(t => t.商品品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品型号)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .HasMaxLength(50);

            this.Property(t => t.营业厅)
                .HasMaxLength(50);

            this.Property(t => t.串码)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            this.Property(t => t.商品编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_Borow");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.审批单).HasColumnName("审批单");
            this.Property(t => t.借贷人).HasColumnName("借贷人");
            this.Property(t => t.借贷日期).HasColumnName("借贷日期");
            this.Property(t => t.借贷部门).HasColumnName("借贷部门");
            this.Property(t => t.借贷方式).HasColumnName("借贷方式");
            this.Property(t => t.商品类别).HasColumnName("商品类别");
            this.Property(t => t.商品品牌).HasColumnName("商品品牌");
            this.Property(t => t.商品型号).HasColumnName("商品型号");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.营业厅).HasColumnName("营业厅");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.商品编码).HasColumnName("商品编码");
        }
    }
}
