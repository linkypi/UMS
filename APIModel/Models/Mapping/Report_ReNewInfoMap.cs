using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_ReNewInfoMap : EntityTypeConfiguration<Report_ReNewInfo>
    {
        public Report_ReNewInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.序号);

            // Properties
            this.Property(t => t.序号)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.续期方式)
                .HasMaxLength(50);

            this.Property(t => t.续期比列)
                .HasMaxLength(101);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            this.Property(t => t.会员姓名)
                .HasMaxLength(50);

            this.Property(t => t.会员电话)
                .HasMaxLength(50);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.拦装人)
                .HasMaxLength(50);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_ReNewInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.续期方式).HasColumnName("续期方式");
            this.Property(t => t.续期比列).HasColumnName("续期比列");
            this.Property(t => t.续期金额).HasColumnName("续期金额");
            this.Property(t => t.续期积分).HasColumnName("续期积分");
            this.Property(t => t.续期天数).HasColumnName("续期天数");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.续期日期).HasColumnName("续期日期");
            this.Property(t => t.会员姓名).HasColumnName("会员姓名");
            this.Property(t => t.会员电话).HasColumnName("会员电话");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.拦装人).HasColumnName("拦装人");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
