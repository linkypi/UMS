using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_ChangeListInfoMap : EntityTypeConfiguration<Report_ChangeListInfo>
    {
        public Report_ChangeListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.系统自增外键编号, t.数量 });

            // Properties
            this.Property(t => t.旧批次号)
                .HasMaxLength(50);

            this.Property(t => t.新批次号)
                .HasMaxLength(50);

            this.Property(t => t.系统自增外键编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.原类别)
                .HasMaxLength(50);

            this.Property(t => t.原品牌)
                .HasMaxLength(50);

            this.Property(t => t.原商品名称)
                .HasMaxLength(50);

            this.Property(t => t.现类别)
                .HasMaxLength(50);

            this.Property(t => t.现品牌)
                .HasMaxLength(50);

            this.Property(t => t.现商品名称)
                .HasMaxLength(50);

            this.Property(t => t.数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.转类别仓库)
                .HasMaxLength(50);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.转类别仓库编码)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_ChangeListInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.旧批次号).HasColumnName("旧批次号");
            this.Property(t => t.新批次号).HasColumnName("新批次号");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.自增外键转类别编号).HasColumnName("自增外键转类别编号");
            this.Property(t => t.原类别).HasColumnName("原类别");
            this.Property(t => t.原品牌).HasColumnName("原品牌");
            this.Property(t => t.原商品名称).HasColumnName("原商品名称");
            this.Property(t => t.原商品属性).HasColumnName("原商品属性");
            this.Property(t => t.现类别).HasColumnName("现类别");
            this.Property(t => t.现品牌).HasColumnName("现品牌");
            this.Property(t => t.现商品名称).HasColumnName("现商品名称");
            this.Property(t => t.现商品属性).HasColumnName("现商品属性");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.转类别仓库).HasColumnName("转类别仓库");
            this.Property(t => t.转类别日期).HasColumnName("转类别日期");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.转类别仓库编码).HasColumnName("转类别仓库编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
            this.Property(t => t.备注).HasColumnName("备注");
        }
    }
}
