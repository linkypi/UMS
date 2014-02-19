using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_AirOutListInfoMap : EntityTypeConfiguration<Report_AirOutListInfo>
    {
        public Report_AirOutListInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.批次号, t.系统自增外键编号, t.调拨金额, t.已接收 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.调拨单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.调出仓库)
                .HasMaxLength(50);

            this.Property(t => t.调入仓库)
                .HasMaxLength(50);

            this.Property(t => t.调拨人)
                .HasMaxLength(50);

            this.Property(t => t.调拨备注)
                .HasMaxLength(50);

            this.Property(t => t.批次号)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.系统自增外键编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.调出号码)
                .HasMaxLength(50);

            this.Property(t => t.调拨金额)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.调入号码)
                .HasMaxLength(50);

            this.Property(t => t.已接收)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.接收人)
                .HasMaxLength(50);

            this.Property(t => t.调出仓库编码)
                .HasMaxLength(50);

            this.Property(t => t.调入仓库编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_AirOutListInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.调拨单号).HasColumnName("调拨单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.调出仓库).HasColumnName("调出仓库");
            this.Property(t => t.调入仓库).HasColumnName("调入仓库");
            this.Property(t => t.调拨日期).HasColumnName("调拨日期");
            this.Property(t => t.调拨人).HasColumnName("调拨人");
            this.Property(t => t.调拨备注).HasColumnName("调拨备注");
            this.Property(t => t.批次号).HasColumnName("批次号");
            this.Property(t => t.系统自增外键编号).HasColumnName("系统自增外键编号");
            this.Property(t => t.调出号码).HasColumnName("调出号码");
            this.Property(t => t.调出号码属性).HasColumnName("调出号码属性");
            this.Property(t => t.调拨金额).HasColumnName("调拨金额");
            this.Property(t => t.调入号码).HasColumnName("调入号码");
            this.Property(t => t.调入号码属性).HasColumnName("调入号码属性");
            this.Property(t => t.已接收).HasColumnName("已接收");
            this.Property(t => t.接收人).HasColumnName("接收人");
            this.Property(t => t.接收日期).HasColumnName("接收日期");
            this.Property(t => t.调出仓库编码).HasColumnName("调出仓库编码");
            this.Property(t => t.调入仓库编码).HasColumnName("调入仓库编码");
            this.Property(t => t.类别编码).HasColumnName("类别编码");
        }
    }
}
