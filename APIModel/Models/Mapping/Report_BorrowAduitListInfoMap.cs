using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Report_BorrowAduitListInfoMap : EntityTypeConfiguration<Report_BorrowAduitListInfo>
    {
        public Report_BorrowAduitListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.系统主键);

            // Properties
            this.Property(t => t.系统主键)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.商品类别)
                .HasMaxLength(50);

            this.Property(t => t.商品品牌)
                .HasMaxLength(50);

            this.Property(t => t.商品型号)
                .HasMaxLength(50);

            this.Property(t => t.备注)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Report_BorrowAduitListInfo");
            this.Property(t => t.系统主键).HasColumnName("系统主键");
            this.Property(t => t.系统外键).HasColumnName("系统外键");
            this.Property(t => t.商品类别).HasColumnName("商品类别");
            this.Property(t => t.商品品牌).HasColumnName("商品品牌");
            this.Property(t => t.商品型号).HasColumnName("商品型号");
            this.Property(t => t.属性).HasColumnName("属性");
            this.Property(t => t.借贷数量).HasColumnName("借贷数量");
            this.Property(t => t.借贷单价).HasColumnName("借贷单价");
            this.Property(t => t.备注).HasColumnName("备注");
        }
    }
}