using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_BorrowAduitInfoMap : EntityTypeConfiguration<Report_BorrowAduitInfo>
    {
        public Report_BorrowAduitInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.系统主键);

            // Properties
            this.Property(t => t.系统主键)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.申请单号)
                .HasMaxLength(50);

            this.Property(t => t.已审批)
                .HasMaxLength(1);

            this.Property(t => t.已通过)
                .HasMaxLength(1);

            this.Property(t => t.已使用)
                .HasMaxLength(1);

            this.Property(t => t.申请人)
                .HasMaxLength(50);

            this.Property(t => t.门店)
                .HasMaxLength(50);

            this.Property(t => t.内部借机)
                .HasMaxLength(1);

            this.Property(t => t.借贷人)
                .HasMaxLength(50);

            this.Property(t => t.借贷方式)
                .HasMaxLength(50);

            this.Property(t => t.借贷部门)
                .HasMaxLength(50);

            this.Property(t => t.借贷人电话)
                .HasMaxLength(50);

            this.Property(t => t.申请备注)
                .HasMaxLength(50);

            this.Property(t => t.一级已审批)
                .HasMaxLength(1);

            this.Property(t => t.一级已通过)
                .HasMaxLength(1);

            this.Property(t => t.一级审批人)
                .HasMaxLength(50);

            this.Property(t => t.一级备注)
                .HasMaxLength(100);

            this.Property(t => t.二级已审批)
                .HasMaxLength(1);

            this.Property(t => t.二级已通过)
                .HasMaxLength(1);

            this.Property(t => t.二级审批人)
                .HasMaxLength(50);

            this.Property(t => t.二级备注)
                .HasMaxLength(100);

            this.Property(t => t.三级已审批)
                .HasMaxLength(1);

            this.Property(t => t.三级已通过)
                .HasMaxLength(1);

            this.Property(t => t.三级审批人)
                .HasMaxLength(50);

            this.Property(t => t.三级备注)
                .HasMaxLength(100);

            this.Property(t => t.四级已审批)
                .HasMaxLength(1);

            this.Property(t => t.四级已通过)
                .HasMaxLength(1);

            this.Property(t => t.四级审批人)
                .HasMaxLength(50);

            this.Property(t => t.四级备注)
                .HasMaxLength(100);

            this.Property(t => t.门店编码)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_BorrowAduitInfo");
            this.Property(t => t.序号).HasColumnName("序号");
            this.Property(t => t.系统主键).HasColumnName("系统主键");
            this.Property(t => t.申请单号).HasColumnName("申请单号");
            this.Property(t => t.借贷总额).HasColumnName("借贷总额");
            this.Property(t => t.已审批).HasColumnName("已审批");
            this.Property(t => t.已通过).HasColumnName("已通过");
            this.Property(t => t.已使用).HasColumnName("已使用");
            this.Property(t => t.使用日期).HasColumnName("使用日期");
            this.Property(t => t.申请人).HasColumnName("申请人");
            this.Property(t => t.申请日期).HasColumnName("申请日期");
            this.Property(t => t.系统日期).HasColumnName("系统日期");
            this.Property(t => t.门店).HasColumnName("门店");
            this.Property(t => t.内部借机).HasColumnName("内部借机");
            this.Property(t => t.借贷人).HasColumnName("借贷人");
            this.Property(t => t.借贷方式).HasColumnName("借贷方式");
            this.Property(t => t.借贷部门).HasColumnName("借贷部门");
            this.Property(t => t.借贷人电话).HasColumnName("借贷人电话");
            this.Property(t => t.预计归还日期).HasColumnName("预计归还日期");
            this.Property(t => t.申请备注).HasColumnName("申请备注");
            this.Property(t => t.一级已审批).HasColumnName("一级已审批");
            this.Property(t => t.一级已通过).HasColumnName("一级已通过");
            this.Property(t => t.一级审批人).HasColumnName("一级审批人");
            this.Property(t => t.一级审批日期).HasColumnName("一级审批日期");
            this.Property(t => t.一级备注).HasColumnName("一级备注");
            this.Property(t => t.二级已审批).HasColumnName("二级已审批");
            this.Property(t => t.二级已通过).HasColumnName("二级已通过");
            this.Property(t => t.二级审批人).HasColumnName("二级审批人");
            this.Property(t => t.二级审批日期).HasColumnName("二级审批日期");
            this.Property(t => t.二级备注).HasColumnName("二级备注");
            this.Property(t => t.三级已审批).HasColumnName("三级已审批");
            this.Property(t => t.三级已通过).HasColumnName("三级已通过");
            this.Property(t => t.三级审批人).HasColumnName("三级审批人");
            this.Property(t => t.三级审批日期).HasColumnName("三级审批日期");
            this.Property(t => t.三级备注).HasColumnName("三级备注");
            this.Property(t => t.四级已审批).HasColumnName("四级已审批");
            this.Property(t => t.四级已通过).HasColumnName("四级已通过");
            this.Property(t => t.四级审批人).HasColumnName("四级审批人");
            this.Property(t => t.四级审批日期).HasColumnName("四级审批日期");
            this.Property(t => t.四级备注).HasColumnName("四级备注");
            this.Property(t => t.门店编码).HasColumnName("门店编码");
        }
    }
}
