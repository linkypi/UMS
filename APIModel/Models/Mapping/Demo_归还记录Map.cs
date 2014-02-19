using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_归还记录Map : EntityTypeConfiguration<Demo_归还记录>
    {
        public Demo_归还记录Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.借贷人)
                .HasMaxLength(50);

            this.Property(t => t.类别)
                .HasMaxLength(50);

            this.Property(t => t.品牌)
                .HasMaxLength(50);

            this.Property(t => t.型号)
                .HasMaxLength(50);

            this.Property(t => t.串号)
                .HasMaxLength(50);

            this.Property(t => t.受理人)
                .HasMaxLength(50);

            this.Property(t => t.营业厅)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_归还记录");
            this.Property(t => t.借贷人).HasColumnName("借贷人");
            this.Property(t => t.类别).HasColumnName("类别");
            this.Property(t => t.品牌).HasColumnName("品牌");
            this.Property(t => t.型号).HasColumnName("型号");
            this.Property(t => t.归还数量).HasColumnName("归还数量");
            this.Property(t => t.售出数量).HasColumnName("售出数量");
            this.Property(t => t.未售出数量_好_).HasColumnName("未售出数量(好)");
            this.Property(t => t.未售出数量_坏_).HasColumnName("未售出数量(坏)");
            this.Property(t => t.串号).HasColumnName("串号");
            this.Property(t => t.借贷日期).HasColumnName("借贷日期");
            this.Property(t => t.归还日期).HasColumnName("归还日期");
            this.Property(t => t.受理人).HasColumnName("受理人");
            this.Property(t => t.营业厅).HasColumnName("营业厅");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
