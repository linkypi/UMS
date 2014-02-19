using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_销售汇总Map : EntityTypeConfiguration<Demo_销售汇总>
    {
        public Demo_销售汇总Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.类别)
                .HasMaxLength(8);

            this.Property(t => t.品牌)
                .HasMaxLength(8);

            this.Property(t => t.机型)
                .HasMaxLength(44);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Demo_销售汇总");
            this.Property(t => t.日期).HasColumnName("日期");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.机型).HasColumnName("机型");
            this.Property(t => t.恒信).HasColumnName("恒信");
            this.Property(t => t.西区).HasColumnName("西区");
            this.Property(t => t.大信中庭).HasColumnName("大信中庭");
            this.Property(t => t.沙朗).HasColumnName("沙朗");
            this.Property(t => t.东区).HasColumnName("东区");
            this.Property(t => t.濠头).HasColumnName("濠头");
            this.Property(t => t.黄圃新明).HasColumnName("黄圃新明");
            this.Property(t => t.大涌华泰店).HasColumnName("大涌华泰店");
            this.Property(t => t.横栏四沙).HasColumnName("横栏四沙");
            this.Property(t => t.江门炮台营业厅).HasColumnName("江门炮台营业厅");
            this.Property(t => t.总数).HasColumnName("总数");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
