using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_OutInfoMap : EntityTypeConfiguration<Report_OutInfo>
    {
        public Report_OutInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.序号, t.系统自增编号, t.已接收 });

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.系统自增编号)
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

            this.Property(t => t.备注)
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
            this.ToTable("Report_OutInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.系统自增编号).HasColumnName("系统自增编号");
            this.Property(t => t.调拨单号).HasColumnName("调拨单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.调出仓库).HasColumnName("调出仓库");
            this.Property(t => t.调入仓库).HasColumnName("调入仓库");
            this.Property(t => t.调拨日期).HasColumnName("调拨日期");
            this.Property(t => t.调拨人).HasColumnName("调拨人");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.已接收).HasColumnName("已接收");
            this.Property(t => t.接收人).HasColumnName("接收人");
            this.Property(t => t.接收日期).HasColumnName("接收日期");
            this.Property(t => t.调出仓库编码).HasColumnName("调出仓库编码");
            this.Property(t => t.调入仓库编码).HasColumnName("调入仓库编码");
            this.Property(t => t.总高度).HasColumnName("总高度");
        }
    }
}
