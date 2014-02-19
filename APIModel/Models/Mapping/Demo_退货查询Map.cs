using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_退货查询Map : EntityTypeConfiguration<Demo_退货查询>
    {
        public Demo_退货查询Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.原类别)
                .HasMaxLength(50);

            this.Property(t => t.原品牌)
                .HasMaxLength(50);

            this.Property(t => t.原型号)
                .HasMaxLength(50);

            this.Property(t => t.原出售价)
                .HasMaxLength(50);

            this.Property(t => t.原差价)
                .HasMaxLength(50);

            this.Property(t => t.出售厅)
                .HasMaxLength(50);

            this.Property(t => t.客户名)
                .HasMaxLength(50);

            this.Property(t => t.售货员)
                .HasMaxLength(50);

            this.Property(t => t.新类别)
                .HasMaxLength(50);

            this.Property(t => t.新品牌)
                .HasMaxLength(50);

            this.Property(t => t.新型号)
                .HasMaxLength(50);

            this.Property(t => t.新价格)
                .HasMaxLength(50);

            this.Property(t => t.新差价)
                .HasMaxLength(50);

            this.Property(t => t.换取数量)
                .HasMaxLength(50);

            this.Property(t => t.实收金额)
                .HasMaxLength(50);

            this.Property(t => t.退货原因)
                .HasMaxLength(50);

            this.Property(t => t.录入人)
                .HasMaxLength(50);

            this.Property(t => t.退换厅)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_退货查询");
            this.Property(t => t.原类别).HasColumnName("原类别");
            this.Property(t => t.原品牌).HasColumnName("原品牌");
            this.Property(t => t.原型号).HasColumnName("原型号");
            this.Property(t => t.退换数).HasColumnName("退换数");
            this.Property(t => t.原出售价).HasColumnName("原出售价");
            this.Property(t => t.原差价).HasColumnName("原差价");
            this.Property(t => t.出售厅).HasColumnName("出售厅");
            this.Property(t => t.客户名).HasColumnName("客户名");
            this.Property(t => t.售货员).HasColumnName("售货员");
            this.Property(t => t.新类别).HasColumnName("新类别");
            this.Property(t => t.新品牌).HasColumnName("新品牌");
            this.Property(t => t.新型号).HasColumnName("新型号");
            this.Property(t => t.新价格).HasColumnName("新价格");
            this.Property(t => t.新差价).HasColumnName("新差价");
            this.Property(t => t.换取数量).HasColumnName("换取数量");
            this.Property(t => t.实收金额).HasColumnName("实收金额");
            this.Property(t => t.退货原因).HasColumnName("退货原因");
            this.Property(t => t.退换日期).HasColumnName("退换日期");
            this.Property(t => t.录入人).HasColumnName("录入人");
            this.Property(t => t.退换厅).HasColumnName("退换厅");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
