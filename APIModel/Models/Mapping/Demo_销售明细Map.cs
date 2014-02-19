using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_销售明细Map : EntityTypeConfiguration<Demo_销售明细>
    {
        public Demo_销售明细Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.原始单号)
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.型号)
                .HasMaxLength(500);

            this.Property(t => t.支付方式)
                .HasMaxLength(50);

            this.Property(t => t.购买方式)
                .HasMaxLength(50);

            this.Property(t => t.销售数量)
                .HasMaxLength(50);

            this.Property(t => t.销售单价)
                .HasMaxLength(50);

            this.Property(t => t.销售差价)
                .HasMaxLength(50);

            this.Property(t => t.销售金额)
                .HasMaxLength(50);

            this.Property(t => t.优惠金额)
                .HasMaxLength(50);

            this.Property(t => t.销售方式)
                .HasMaxLength(50);

            this.Property(t => t.售货员)
                .HasMaxLength(50);

            this.Property(t => t.出售日期)
                .HasMaxLength(50);

            this.Property(t => t.营业厅)
                .HasMaxLength(50);

            this.Property(t => t.客户名称)
                .HasMaxLength(50);

            this.Property(t => t.联系电话)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Demo_销售明细");
            this.Property(t => t.原始单号).HasColumnName("原始单号");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.型号).HasColumnName("型号");
            this.Property(t => t.支付方式).HasColumnName("支付方式");
            this.Property(t => t.购买方式).HasColumnName("购买方式");
            this.Property(t => t.销售数量).HasColumnName("销售数量");
            this.Property(t => t.销售单价).HasColumnName("销售单价");
            this.Property(t => t.销售差价).HasColumnName("销售差价");
            this.Property(t => t.销售金额).HasColumnName("销售金额");
            this.Property(t => t.优惠金额).HasColumnName("优惠金额");
            this.Property(t => t.销售方式).HasColumnName("销售方式");
            this.Property(t => t.售货员).HasColumnName("售货员");
            this.Property(t => t.出售日期).HasColumnName("出售日期");
            this.Property(t => t.营业厅).HasColumnName("营业厅");
            this.Property(t => t.客户名称).HasColumnName("客户名称");
            this.Property(t => t.联系电话).HasColumnName("联系电话");
            this.Property(t => t.备注).HasColumnName("备注");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
