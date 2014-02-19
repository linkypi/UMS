using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_进销存报表Map : EntityTypeConfiguration<Demo_进销存报表>
    {
        public Demo_进销存报表Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.型号)
                .HasMaxLength(50);

            this.Property(t => t.期初)
                .HasMaxLength(50);

            this.Property(t => t.营业厅)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_进销存报表");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.型号).HasColumnName("型号");
            this.Property(t => t.期初).HasColumnName("期初");
            this.Property(t => t.本期初始入库).HasColumnName("本期初始入库");
            this.Property(t => t.调入).HasColumnName("调入");
            this.Property(t => t.本期调出).HasColumnName("本期调出");
            this.Property(t => t.本期销售).HasColumnName("本期销售");
            this.Property(t => t.本期送修).HasColumnName("本期送修");
            this.Property(t => t.本期借贷).HasColumnName("本期借贷");
            this.Property(t => t.期末库存).HasColumnName("期末库存");
            this.Property(t => t.送修累计).HasColumnName("送修累计");
            this.Property(t => t.本期返库).HasColumnName("本期返库");
            this.Property(t => t.借贷累计).HasColumnName("借贷累计");
            this.Property(t => t.本期归还).HasColumnName("本期归还");
            this.Property(t => t.营业厅).HasColumnName("营业厅");
            this.Property(t => t.开始时间).HasColumnName("开始时间");
            this.Property(t => t.结束时间).HasColumnName("结束时间");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
