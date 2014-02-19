using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_ReturnListInfoMap : EntityTypeConfiguration<Report_ReturnListInfo>
    {
        public Report_ReturnListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.系统自增主键编码, t.系统自增外键编号 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.系统自增主键编码)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.系统自增外键编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.借贷单号)
                .HasMaxLength(50);

            this.Property(t => t.审批单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.借贷门店)
                .HasMaxLength(50);

            this.Property(t => t.借贷部门)
                .HasMaxLength(50);

            this.Property(t => t.借贷人)
                .HasMaxLength(50);

            this.Property(t => t.联系电话)
                .HasMaxLength(50);

            this.Property(t => t.借贷方式)
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品名称)
                .HasMaxLength(50);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.借贷仓库编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_ReturnListInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.系统自增主键编码).HasColumnName("系统自增主键编码");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.借贷单号).HasColumnName("借贷单号");
            this.Property(t => t.审批单号).HasColumnName("审批单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.借贷门店).HasColumnName("借贷门店");
            this.Property(t => t.借贷日期).HasColumnName("借贷日期");
            this.Property(t => t.借贷部门).HasColumnName("借贷部门");
            this.Property(t => t.借贷人).HasColumnName("借贷人");
            this.Property(t => t.联系电话).HasColumnName("联系电话");
            this.Property(t => t.借贷方式).HasColumnName("借贷方式");
            this.Property(t => t.预计归还日期).HasColumnName("预计归还日期");
            this.Property(t => t.归还日期).HasColumnName("归还日期");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.商品名称).HasColumnName("商品名称");
            this.Property(t => t.商品属性).HasColumnName("商品属性");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.单价).HasColumnName("单价");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.借贷仓库编码).HasColumnName("借贷仓库编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
