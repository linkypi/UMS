using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_InOrderInfoMap : EntityTypeConfiguration<Report_InOrderInfo>
    {
        public Report_InOrderInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.系统自增编号);

            // Properties
            this.Property(t => t.系统自增编号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.入库单号)
                .HasMaxLength(50);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.入库仓库)
                .HasMaxLength(50);

            this.Property(t => t.操作人)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.入库仓库编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_InOrderInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.系统自增编号).HasColumnName("系统自增编号");
            this.Property(t => t.入库单号).HasColumnName("入库单号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.入库仓库).HasColumnName("入库仓库");
            this.Property(t => t.入库日期).HasColumnName("入库日期");
            this.Property(t => t.操作人).HasColumnName("操作人");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.入库仓库编码).HasColumnName("入库仓库编码");
        }
    }
}
