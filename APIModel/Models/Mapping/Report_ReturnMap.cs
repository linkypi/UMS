using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_ReturnMap : EntityTypeConfiguration<Report_Return>
    {
        public Report_ReturnMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.商品类别)
                .HasMaxLength(50);

            this.Property(t => t.商品品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品型号)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .HasMaxLength(50);

            this.Property(t => t.串码)
                .HasMaxLength(50);

            this.Property(t => t.归还日期)
                .HasMaxLength(100);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_Return");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.借贷单号).HasColumnName("借贷单号");
            this.Property(t => t.商品类别).HasColumnName("商品类别");
            this.Property(t => t.商品品牌).HasColumnName("商品品牌");
            this.Property(t => t.商品型号).HasColumnName("商品型号");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.归还日期).HasColumnName("归还日期");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
