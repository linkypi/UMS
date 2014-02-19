using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_SMSSignMap : EntityTypeConfiguration<Report_SMSSign>
    {
        public Report_SMSSignMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.合同金额, t.合同发送数量, t.实收金额, t.实际发送数量 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.销售单号)
                .HasMaxLength(50);

            this.Property(t => t.行业)
                .HasMaxLength(50);

            this.Property(t => t.单位名称)
                .HasMaxLength(50);

            this.Property(t => t.单位地址)
                .HasMaxLength(500);

            this.Property(t => t.业务内容)
                .HasMaxLength(50);

            this.Property(t => t.合同金额)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.合同发送数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.实收金额)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.实际发送数量)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.联系人)
                .HasMaxLength(50);

            this.Property(t => t.联系电话)
                .HasMaxLength(50);

            this.Property(t => t.发票抬头)
                .HasMaxLength(50);

            this.Property(t => t.发票代码)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.录单员)
                .HasMaxLength(50);

            this.Property(t => t.销售员)
                .HasMaxLength(50);

            this.Property(t => t.仓库)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_SMSSign");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.销售单号).HasColumnName("销售单号");
            this.Property(t => t.系统时间).HasColumnName("系统时间");
            this.Property(t => t.行业).HasColumnName("行业");
            this.Property(t => t.合同日期).HasColumnName("合同日期");
            this.Property(t => t.单位名称).HasColumnName("单位名称");
            this.Property(t => t.单位地址).HasColumnName("单位地址");
            this.Property(t => t.业务内容).HasColumnName("业务内容");
            this.Property(t => t.合同金额).HasColumnName("合同金额");
            this.Property(t => t.合同发送数量).HasColumnName("合同发送数量");
            this.Property(t => t.实收金额).HasColumnName("实收金额");
            this.Property(t => t.实际发送数量).HasColumnName("实际发送数量");
            this.Property(t => t.合同结清日期).HasColumnName("合同结清日期");
            this.Property(t => t.实际结清日期).HasColumnName("实际结清日期");
            this.Property(t => t.佣金).HasColumnName("佣金");
            this.Property(t => t.联系人).HasColumnName("联系人");
            this.Property(t => t.联系电话).HasColumnName("联系电话");
            this.Property(t => t.发票抬头).HasColumnName("发票抬头");
            this.Property(t => t.发票代码).HasColumnName("发票代码");
            this.Property(t => t.发票日期).HasColumnName("发票日期");
            this.Property(t => t.税率).HasColumnName("税率");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.已结清).HasColumnName("已结清");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.录单员).HasColumnName("录单员");
            this.Property(t => t.销售员).HasColumnName("销售员");
            this.Property(t => t.仓库).HasColumnName("仓库");
        }
    }
}
