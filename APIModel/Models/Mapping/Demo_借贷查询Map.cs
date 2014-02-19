using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_借贷查询Map : EntityTypeConfiguration<Demo_借贷查询>
    {
        public Demo_借贷查询Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.标识列)
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.型号)
                .HasMaxLength(50);

            this.Property(t => t.未归还借机)
                .HasMaxLength(50);

            this.Property(t => t.串号)
                .HasMaxLength(50);

            this.Property(t => t.借贷人)
                .HasMaxLength(50);

            this.Property(t => t.所属单位)
                .HasMaxLength(50);

            this.Property(t => t.所属部门)
                .HasMaxLength(50);

            this.Property(t => t.受理人)
                .HasMaxLength(50);

            this.Property(t => t.联系方式)
                .HasMaxLength(50);

            this.Property(t => t.借贷周期_天)
                .HasMaxLength(50);

            this.Property(t => t.营业厅)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_借贷查询");
            this.Property(t => t.标识列).HasColumnName("标识列");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.型号).HasColumnName("型号");
            this.Property(t => t.数量).HasColumnName("数量");
            this.Property(t => t.销售价格).HasColumnName("销售价格");
            this.Property(t => t.已还数量).HasColumnName("已还数量");
            this.Property(t => t.未归还借机).HasColumnName("未归还借机");
            this.Property(t => t.串号).HasColumnName("串号");
            this.Property(t => t.借贷人).HasColumnName("借贷人");
            this.Property(t => t.所属单位).HasColumnName("所属单位");
            this.Property(t => t.所属部门).HasColumnName("所属部门");
            this.Property(t => t.受理人).HasColumnName("受理人");
            this.Property(t => t.联系方式).HasColumnName("联系方式");
            this.Property(t => t.借贷日期).HasColumnName("借贷日期");
            this.Property(t => t.借贷周期_天).HasColumnName("借贷周期 天");
            this.Property(t => t.营业厅).HasColumnName("营业厅");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
