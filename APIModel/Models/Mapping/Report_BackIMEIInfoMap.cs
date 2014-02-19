using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_BackIMEIInfoMap : EntityTypeConfiguration<Report_BackIMEIInfo>
    {
        public Report_BackIMEIInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.系统自增编号, t.系统自增外键编号, t.数量 });

            // Properties
            this.Property(t => t.系统自增编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.批次号)
                .HasMaxLength(50);

            this.Property(t => t.系统自增外键编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.串码)
                .HasMaxLength(50);

            this.Property(t => t.退库单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.退库仓库)
                .HasMaxLength(50);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.退库仓库编码)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_BackIMEIInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.系统自增编号).HasColumnName("系统自增编号");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.自增外键退库单编号).HasColumnName("自增外键退库单编号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.串码).HasColumnName("串码");
            this.Property(t => t.退库单号).HasColumnName("退库单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.退库仓库).HasColumnName("退库仓库");
            this.Property(t => t.退库日期).HasColumnName("退库日期");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.退库仓库编码).HasColumnName("退库仓库编码");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
